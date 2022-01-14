using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KSaveDataMan
{
    /// <summary>
    /// Three operations: clear, save, load with string[] keys
    /// At the begining of each we do a sync Check operation so that internal master data is available
    /// Then we find json locator for typeName and keys.
    /// Then for clear-->If locator is not found, ignore further
    /// Then for save-->If we find json locator, overwrite the byte array in the file path async way. 
    ///                 If not found, create a locator in master data and the write byte array in file path async way.
    /// Then for load-->If we find json locator, we load byte array from device using file path in locator. 
    ///                 For valid byte array, we cast it to actual data using Json util of unity. Otherwise a null is returned.
    ///              -->If not found, create a locator in master data and save a default valued json in file path with byte array
    /// </summary>
    internal static class ClassSave
    {
        static ClassSaveMasterData masterSave = null;

        static ClassSaveMasterData GetDefaultMasterSave()
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

        static void EnsureMasterSave()
        {
            if (masterSave == null)
            {
                LoadMasterSave();
                WriteMasterSave();
            }

            void LoadMasterSave()
            {
                var fPath = ClassSaveUtil.GetMasterSaveFilePath();
                if (File.Exists(fPath))
                {
                    var saveBytes = File.ReadAllBytes(fPath);
                    var masterSaveJson = CryptoUtil.DecryptIfRequired(saveBytes);
                    masterSave = Util.GetDataFromJson<ClassSaveMasterData>(masterSaveJson);
                }
                else
                {
                    masterSave = GetDefaultMasterSave();
                }
            }
        }

        internal static void WriteMasterSave()
        {
            if (masterSave == null)
            {
                masterSave = GetDefaultMasterSave();
            }
            string json = "";
            try
            {
                json = JsonUtility.ToJson(masterSave);
            }
            catch (System.Exception ex)
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogError("Can not convert the master save data for class save into json. Exception message: " + ex.Message);
                }
            }

            byte[] saveBytes = null;
            try
            {
                saveBytes = CryptoUtil.EncryptIfRequired(json);
            }
            catch (System.Exception ex)
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogError("Byte conversion error during class save master data write to device. " +
                        "This is most likely due to invalid encryption operation. Exception message: " + ex.Message);
                }
            }
            var fPath = ClassSaveUtil.GetMasterSaveFilePath();
            try
            {
                File.WriteAllBytes(fPath, saveBytes);
            }
            catch (System.Exception ex)
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogError("Can not write the class master save file into disk! Exception message: " + ex.Message);
                }
            }
        }

        internal static void Clear(string[] keys, System.Action<LocalDataCode> OnComplete)
        {
            EnsureMasterSave();
            if (masterSave != null && masterSave.data != null && masterSave.data.Length > 0)
            {
                
            }
        }

        internal static void Load<T>(string[] keys, System.Action<LocalDataCode, T> OnComplete)
        {
            EnsureMasterSave();

        }

        internal static void Save<T>(T data, string[] keys, System.Action<LocalDataCode> OnComplete)
        {
            EnsureMasterSave();

        }
    }
}
