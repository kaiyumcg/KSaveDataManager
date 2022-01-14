using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KSaveDataMan
{
    internal static class Util 
    {
        internal static T GetDefaultMasterSave<T>() where T : MasterSaveJsonDefault
        {
            var sv = new MasterSaveJsonDefault()
            {
                buildGUID = Application.buildGUID,
                companyName = Application.companyName,
                gameBundleID = Application.identifier,
                gameName = Application.productName,
                gameVer = Application.version,
                unityVer = Application.unityVersion,
                genuine = Application.genuine,
                platform = Application.platform.ToString(),
                encoding = EncodingType.ASCII
            };

            if (Config.data != null)
            {
                sv.encoding = Config.data._EncodingType;
            }
            return (T)(object)sv;
        }

        internal static string GetMasterClassSaveFilePath()
        {
            var saveFileName = "classSaveMaster.sv";
            var saveDirectory = Application.persistentDataPath;
            if (Config.data != null && string.IsNullOrEmpty(Config.data.ClassSaveMasterFileName) == false)
            {
                saveFileName = Config.data.ClassSaveMasterFileName;
                saveDirectory = Config.data.SavePath == SaveFilePathMode.AppDataPath ? Application.dataPath : Application.persistentDataPath;
            }
            var fPath = Path.Combine(saveDirectory, saveFileName);
            return fPath;
        }

        internal static string GetMasterAtomicSaveFilePath()
        {
            var saveFileName = "atom.sv";
            var saveDirectory = Application.persistentDataPath;
            if (Config.data != null && string.IsNullOrEmpty(Config.data.AtomicMasterSaveFileName) == false)
            {
                saveFileName = Config.data.AtomicMasterSaveFileName;
                saveDirectory = Config.data.SavePath == SaveFilePathMode.AppDataPath ? Application.dataPath : Application.persistentDataPath;
            }
            var fPath = Path.Combine(saveDirectory, saveFileName);
            return fPath;
        }

        internal static string GetKeyedRandomName(string[] keys)
        {
            var resStr = "sampleSV";
            if (keys != null && keys.Length > 0)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    var k = keys[i];
                    if (string.IsNullOrEmpty(k)) { continue; }
                    resStr += k + "_";
                }
            }
            resStr = CryptoUtil.EncryptStringAES(resStr);
            return resStr;
        }

        internal static string GetSaveDirectory()
        {
            var dir = Application.persistentDataPath;
            if (Config.data != null) 
            {
                dir = Config.data.SavePath == SaveFilePathMode.AppDataPath ? Application.dataPath : Application.persistentDataPath;
            }
            return dir;
        }

        internal static T GetDataFromJson<T>(string json)
        {
            T result = default;
            try
            {
                result = JsonUtility.FromJson<T>(json);
            }
            catch (System.Exception ex)
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogWarning("Could not get data of type " + typeof(T) + " from json string. " +
                        "Exception is thrown! Err message: " + ex.Message);
                }
            }
            return result;
        }

        internal static string GetDataAsJsonString<T>(T objectToBeJsonized)
        {
            string result = default;
            try
            {
                result = JsonUtility.ToJson(objectToBeJsonized, Config.data == null ? false : Config.data.JsonPettyPrint);
            }
            catch (System.Exception ex)
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogWarning("Could not convert data of type " + typeof(T) + " to json string. " +
                        "Exception is thrown! Err message: " + ex.Message);
                }
            }
            return result;
        }

        internal static string[] CPY(string[] input)
        {
            var result = new string[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = input[i];
            }
            return result;
        }

        internal static bool ExistIn(string[] keys, string key)
        {
            var exist = false;
            if (string.IsNullOrEmpty(key)) { return exist; }
            if (keys != null && keys.Length > 0)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    var k = keys[i];
                    if (string.IsNullOrEmpty(k)) { continue; }
                    if (k == key)
                    {
                        exist = true;
                        break;
                    }
                }
            }
            return exist;
        }

        internal static bool AreBothSame(string[] collection1, string[] collection2, 
            string collectionTypeName1, string collectionTypeName2, bool considerCollectionType)
        {
            var match = false;
            if (considerCollectionType && collectionTypeName1 != collectionTypeName2) { return match; }
            var collection2Len = collection2 == null ? 0 : collection2.Length;
            var collection1Len = collection1 == null ? 0 : collection1.Length;
            if (collection2Len != collection1Len) { return match; }
            if (collection2 != null && collection2.Length > 0)
            {
                for (int i = 0; i < collection2.Length; i++)
                {
                    var c2 = collection2[i];
                    if (string.IsNullOrEmpty(c2)) { continue; }
                    if (ExistIn(collection1, c2))
                    {
                        match = true;
                        break;
                    }
                }
            }
            return match;
        }
    }
}