using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    //On load operation:-->check if master is in memory, if not create one and insert "keyed default T json" data to it and write it to device
    //if master is present in memory then check if "keyed default T json exist", if exist load it async with async file writer
    //otherwise create a fresh one and save it into disk
    internal static class ClassSave
    {
        internal static ClassSaveMasterData classSaveInternal = null;

        internal static ClassSaveMasterData GetDefaultSave()
        {
            var sv = new ClassSaveMasterData()
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

        static void Check<T>(string[] keys)
        {
            if (SaveState.operationManager == null)
            {
                var g = new GameObject("_SaveDataOperationManager_Gen_");
                SaveState.operationManager = g.AddComponent<SaveDataOperationManager>();
            }

            if (ClassSave.classSaveInternal == null)
            {
                AtomicSaveInternalController.LoadFromDevice();
            }
            TryCreateIfNotExist(ref classSaveInternal, keys);
        }

        static void TryCreateIfNotExist(ref ClassSaveMasterData saves_Internal, string[] keys)
        {
            
        }

        internal static void WriteMasterClassSaveToDevice()
        {
            var usePlayerPref = Config.data == null ? false : Config.data.UseUnityPlayerPrefForAtomic;
            if (usePlayerPref) { PlayerPrefs.Save(); }
            else
            {
                if (ClassSave.saveInternal == null)
                {
                    ClassSave.saveInternal = GetDefaultSave();
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

        internal static void LoadMasterClassSaveFromDevice()
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
                WriteToDevice();
            }
        }
    }
}
