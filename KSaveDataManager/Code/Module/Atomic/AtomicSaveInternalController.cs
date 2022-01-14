using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

namespace KSaveDataMan
{
    internal static class AtomicSaveInternalController
    {
        static AtomicSaveMasterData masterSave = null;
        internal static void EnsureMasterSave()
        {
            if (masterSave == null)
            {
                LoadMasterSaveToMemory();
                WriteMasterSaveToDevice();
            }
        }

        internal static void WriteMasterSaveToDevice()
        {
            var usePlayerPref = Config.data == null ? false : Config.data.UseUnityPlayerPrefForAtomic;
            if (usePlayerPref) { PlayerPrefs.Save(); }
            else
            {
                if (masterSave == null) 
                {
                    masterSave = Util.GetDefaultMasterSave<AtomicSaveMasterData>();
                    masterSave.data = null;
                }
                string json = Util.GetDataAsJsonString(masterSave);
                byte[] saveBytes = CryptoUtil.EncryptIfRequired(json);
                var fPath = Util.GetMasterAtomicSaveFilePath();
                FileWriteUtil.WriteData(saveBytes, fPath);
            }
        }

        internal static void LoadMasterSaveToMemory()
        {
            var fPath = Util.GetMasterAtomicSaveFilePath();
            if (File.Exists(fPath))
            {
                var saveBytes = FileLoadUtil.LoadData(fPath);
                var saveJson = CryptoUtil.DecryptIfRequired(saveBytes);
                masterSave = Util.GetDataFromJson<AtomicSaveMasterData>(saveJson);
            }
            else
            {
                masterSave = Util.GetDefaultMasterSave<AtomicSaveMasterData>();
                masterSave.data = null;
                WriteMasterSaveToDevice();
            }
        }

        internal static void Clear(string[] keys)
        {
            if (keys == null)
            {
                masterSave = Util.GetDefaultMasterSave<AtomicSaveMasterData>();
                masterSave.data = null;
            }
            else
            {
                AtomUtil.TryDeleteIfExist(ref masterSave, keys);
            }
            WriteMasterSaveToDevice();
        }

        internal static string GetAtomic<T>(string[] keys)
        {
            EnsureMasterSave();
            AtomUtil.TryCreateIfNotExist(ref masterSave, keys, typeof(T).Name);
            var result = "";
            if (Config.data != null && Config.data.EncryptionConfig != null && Config.data.EncryptionConfig.AtomicEncryptEachData)
            {
                var encryptedData = "";
                AtomUtil.TryGetData(ref masterSave, keys, ref encryptedData);
                var sharedSecret = Config.data.EncryptionConfig.AtomicPerDataSharedSecret;
                var salt = Config.data.EncryptionConfig.AtomicPerDataSalt;
                var encoding = Config.data.EncryptionConfig.AtomicPerDataCredentialEncoding;
                result = CryptoUtil.DecryptStringAES(encryptedData, sharedSecret, encoding, salt);
            }
            else
            {
                AtomUtil.TryGetData(ref masterSave, keys, ref result);
            }
            return result;
        }

        internal static void SetAtomic<T>(string data, string[] keys)
        {
            EnsureMasterSave();
            AtomUtil.TryCreateIfNotExist(ref masterSave, keys, typeof(T).Name);
            if (string.IsNullOrEmpty(data))
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogWarning("Can not save empty data into atomic save");
                }
            }
            else
            {
                if (Config.data != null && Config.data.EncryptionConfig != null && Config.data.EncryptionConfig.AtomicEncryptEachData)
                {
                    var sharedSecret = Config.data.EncryptionConfig.AtomicPerDataSharedSecret;
                    var salt = Config.data.EncryptionConfig.AtomicPerDataSalt;
                    var encoding = Config.data.EncryptionConfig.AtomicPerDataCredentialEncoding;
                    var encryptedData = CryptoUtil.EncryptStringAES(data, sharedSecret, encoding, salt);
                    AtomUtil.TrySetData(ref masterSave, keys, typeof(T).Name, encryptedData);
                }
                else
                {
                    AtomUtil.TrySetData(ref masterSave, keys, typeof(T).Name, data);
                }
            }
        }

        internal static bool HasData<T>(string[] keys)
        {
            EnsureMasterSave();
            bool existed = false;
            AtomUtil.TryCheckIfDataExist(ref masterSave, keys, ref existed);
            return existed;
        }
    }
}