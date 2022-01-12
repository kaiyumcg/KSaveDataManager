using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

namespace KSaveDataMan
{
    internal static class AtomicInternal
    {
        static AtomicSaves_Internal saveInternal = null;
        internal static AtomicSaves_Internal GetDefaultSave()
        {
            var sv = new AtomicSaves_Internal()
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

            if (AtomicInternal.saveInternal == null)
            {
                AtomicInternal.saveInternal = AtomicInternal.GetDefaultSave();
                AtomicInternal.WriteToDevice();
                AtomicInternal.LoadFromDevice();
            }

            var str = "";
            AtomUtil.SaveInternalOperation(ref saveInternal, ref str, keys, AtomicInternalOpMode.Creation, typeof(T).Name);
        }

        internal static void WriteToDevice()
        {
            var usePlayerPref = Config.data == null ? false : Config.data.UseUnityPlayerPrefForAtomic;
            if (usePlayerPref) { PlayerPrefs.Save(); }
            else
            {
                if (AtomicInternal.saveInternal == null) 
                {
                    AtomicInternal.saveInternal = GetDefaultSave();
                }
                string json = "";
                try
                {
                    json = JsonUtility.ToJson(AtomicInternal.saveInternal);
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
                    saveBytes = AtomUtil.EncryptIfReq(json, key, iv);
                }
                catch (System.Exception ex)
                {
                    if (Config.data != null && Config.data.DebugMessage)
                    {
                        Debug.LogError("Byte conversion error. This is most likely due to invalid encryption operation. Exception message: " + ex.Message);
                    }
                }
                var fPath = AtomUtil.GetAtomicSavePath();
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
            var fPath = AtomUtil.GetAtomicSavePath();
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
                var saveJson = AtomUtil.DecryptIfReq(saveBytes, key, iv);
                saveInternal = AtomUtil.GetDataFromJson<AtomicSaves_Internal>(saveJson);
            }
            else
            {
                saveInternal = GetDefaultSave();
                WriteToDevice();
            }
        }

        internal static void DeleteData(string[] keys)
        {
            if (keys == null)
            {
                saveInternal = GetDefaultSave();
                WriteToDevice();
            }
            else
            {
                var str = "";
                AtomUtil.SaveInternalOperation(ref saveInternal, ref str, keys, AtomicInternalOpMode.Delete);
            }
        }

        internal static string GetAtomic<T>(string[] keys)
        {
            Check<T>(keys);
            var result = "";
            var str = "";
            AtomUtil.SaveInternalOperation(ref saveInternal, ref str, keys, AtomicInternalOpMode.Read, typeof(T).Name);
            if (string.IsNullOrEmpty(str))
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogWarning("There is no data for given key inside atomic save file. ");
                }
            }
            else
            {
                var key = "";
                var iv = "";
                if (Config.data != null && Config.data.EncryptionConfig != null && Config.data.EncryptionConfig.EncryptEachData)
                {
                    key = Config.data.EncryptionConfig.UseDifferentCredentialForEachDataEncryption ? Config.data.EncryptionConfig.PerDataKey :
                        Config.data.EncryptionConfig.KEY;
                    iv = Config.data.EncryptionConfig.UseDifferentCredentialForEachDataEncryption ? Config.data.EncryptionConfig.PerDataIV :
                        Config.data.EncryptionConfig.IV;
                }
                var b_str = AtomUtil.ConvertTo(str);
                result = AtomUtil.DecryptIfReq(b_str, key, iv);
            }
            return result;
        }

        internal static void SetAtomic<T>(string data, string[] keys)
        {
            Check<T>(keys);
            //if its empty string, throw error if debugMessage is enabled
            //if encryption setting is on per data then encrypt the data
            //set into field for corresponding keys

            var result = "";
            var str = data;
            AtomUtil.SaveInternalOperation(ref saveInternal, ref str, keys, AtomicInternalOpMode.Write, typeof(T).Name);
            if (string.IsNullOrEmpty(str))
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogWarning("Can not save empty data into atomic save");
                }
            }
            else
            {
                var key = "";
                var iv = "";
                if (Config.data != null && Config.data.EncryptionConfig != null && Config.data.EncryptionConfig.EncryptEachData)
                {
                    key = Config.data.EncryptionConfig.UseDifferentCredentialForEachDataEncryption ? Config.data.EncryptionConfig.PerDataKey :
                        Config.data.EncryptionConfig.KEY;
                    iv = Config.data.EncryptionConfig.UseDifferentCredentialForEachDataEncryption ? Config.data.EncryptionConfig.PerDataIV :
                        Config.data.EncryptionConfig.IV;
                }
                var b_str = AtomUtil.ConvertTo(str);
                var convData = AtomUtil.EncryptIfReq(data, key, iv);
            }
        }

        internal static bool HasData<T>(string[] keys)
        {
            Check<T>(keys);
            //there should be data against keys, if not then no data
            //if there is data but the wrote flag is false then no data
            //true otherwise
            throw new System.NotImplementedException();
        }
    }
}