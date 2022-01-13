using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

namespace KSaveDataMan
{
    internal static class AtomicSaveInternalController
    {
        static AtomicSaveMasterData saveInternal = null;
        internal static AtomicSaveMasterData GetDefaultSave()
        {
            var sv = new AtomicSaveMasterData()
            {
                buildGUID = Application.buildGUID,
                companyName = Application.companyName,
                gameBundleID = Application.identifier,
                gameName = Application.productName,
                gameVer = Application.version,
                unityVer = Application.unityVersion,
                genuine = Application.genuine,
                platform = Application.platform.ToString(),
                encoding = EncodingType.ASCII,
                data = null
            };

            if (Config.data != null)
            {
                sv.encoding = Config.data._EncodingType;
            }
            return sv;
        }

        internal static void Check<T>(string[] keys)
        {
            if (SaveState.operationManager == null)
            {
                var g = new GameObject("_SaveDataOperationManager_Gen_");
                SaveState.operationManager = g.AddComponent<SaveDataOperationManager>();
            }

            if (AtomicSaveInternalController.saveInternal == null)
            {
                AtomicSaveInternalController.LoadFromDevice();
            }
            AtomUtil.TryCreateIfNotExist(ref saveInternal, keys, typeof(T).Name);
        }

        internal static void WriteMasterAtomicSaveDataToDevice()
        {
            var usePlayerPref = Config.data == null ? false : Config.data.UseUnityPlayerPrefForAtomic;
            if (usePlayerPref) { PlayerPrefs.Save(); }
            else
            {
                if (AtomicSaveInternalController.saveInternal == null) 
                {
                    AtomicSaveInternalController.saveInternal = GetDefaultSave();
                }
                string json = "";
                try
                {
                    json = JsonUtility.ToJson(AtomicSaveInternalController.saveInternal);
                }
                catch (System.Exception ex)
                {
                    if (Config.data != null && Config.data.DebugMessage) 
                    { 
                        Debug.LogError("Can not convert the save data into json. Exception message: " + ex.Message); 
                    }
                }
                byte[] saveBytes = null;
                try
                {
                    var key = "";
                    var iv = "";
                    if (Config.data != null && Config.data.EncryptionConfig != null) 
                    { 
                        key = Config.data.EncryptionConfig.KEY;
                        iv = Config.data.EncryptionConfig.IV;
                    }
                    saveBytes = CryptoUtil.EncryptIfSettingPermitsOtherwisePaintTxt(json, key, iv);
                }
                catch (System.Exception ex)
                {
                    if (Config.data != null && Config.data.DebugMessage)
                    {
                        Debug.LogError("Byte conversion error. This is most likely due to invalid encryption operation. Exception message: " + ex.Message);
                    }
                }
                var fPath = AtomUtil.GetMasterAtomicSaveFilePath();
                try
                {
                    File.WriteAllBytes(fPath, saveBytes);
                }
                catch (System.Exception ex)
                {
                    if (Config.data != null && Config.data.DebugMessage)
                    {
                        Debug.LogError("Can not write the atomic save file into disk! Exception message: " + ex.Message);
                    }
                }
            }
        }

        internal static void LoadFromDevice()
        {
            var fPath = AtomUtil.GetMasterAtomicSaveFilePath();
            if (File.Exists(fPath))
            {
                var saveBytes = File.ReadAllBytes(fPath);
                var key = "";
                var iv = "";
                if (Config.data != null && Config.data.EncryptionConfig != null)
                {
                    key = Config.data.EncryptionConfig.KEY;
                    iv = Config.data.EncryptionConfig.IV;
                }
                var saveJson = CryptoUtil.DecryptIfSettingPermitsOtherwisePaintTxt(saveBytes, key, iv);
                saveInternal = Util.GetDataFromJson<AtomicSaveMasterData>(saveJson);
            }
            else
            {
                saveInternal = GetDefaultSave();
                WriteMasterAtomicSaveDataToDevice();
            }
        }

        internal static void DeleteData(string[] keys)
        {
            if (keys == null)
            {
                saveInternal = GetDefaultSave();
            }
            else
            {
                AtomUtil.TryDeleteIfExist(ref saveInternal, keys);
            }
            WriteMasterAtomicSaveDataToDevice();
        }

        internal static string GetAtomic<T>(string[] keys)
        {
            Check<T>(keys);
            var result = "";
            if (Config.data != null && Config.data.EncryptionConfig != null && Config.data.EncryptionConfig.AtomicEncryptEachData)
            {
                var encryptedData = "";
                AtomUtil.TryGetData(ref saveInternal, keys, ref encryptedData);
                var sharedSecret = Config.data.EncryptionConfig.AtomicPerDataSharedSecret;
                var salt = Config.data.EncryptionConfig.AtomicPerDataSalt;
                var encoding = Config.data.EncryptionConfig.AtomicPerDataCredentialEncoding;
                result = CryptoUtil.DecryptStringAES(encryptedData, sharedSecret, encoding, salt);
            }
            else
            {
                AtomUtil.TryGetData(ref saveInternal, keys, ref result);
            }
            return result;
        }

        internal static void SetAtomic<T>(string data, string[] keys)
        {
            Check<T>(keys);
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
                    AtomUtil.TrySetData(ref saveInternal, keys, typeof(T).Name, encryptedData);
                }
                else
                {
                    AtomUtil.TrySetData(ref saveInternal, keys, typeof(T).Name, data);
                }
            }
        }

        internal static bool HasData<T>(string[] keys)
        {
            Check<T>(keys);
            bool existed = false;
            AtomUtil.TryCheckIfDataExist(ref saveInternal, keys, ref existed);
            return existed;
        }
    }
}