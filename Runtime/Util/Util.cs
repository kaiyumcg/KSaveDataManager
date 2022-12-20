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

        internal static string GetMasterSaveFilePathForBigData()
        {
            var saveFileName = "bigData.sv";
            var saveDirectory = Application.persistentDataPath;
            if (Config.data != null && string.IsNullOrEmpty(Config.data.BigDataSaveMasterFileName) == false)
            {
                saveFileName = Config.data.BigDataSaveMasterFileName;
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

        internal static string GetStructuredSaveFileExtension()
        {
            var ext = ".SVST";
            if (Config.data != null && string.IsNullOrEmpty(Config.data.StructuredSaveExtension) == false)
            {
                ext = Config.data.StructuredSaveExtension;
            }
            return ext;
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

        internal static T GetAtomicValueFromStringOrJson<T>(string vl)
        {
            T result = default;
            if (typeof(T) == typeof(int))
            {
                int res;
                int.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(long))
            {
                long res;
                long.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(ulong))
            {
                ulong res;
                ulong.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(short))
            {
                short res;
                short.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(ushort))
            {
                ushort res;
                ushort.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(uint))
            {
                uint res;
                uint.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(bool))
            {
                bool res;
                bool.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(byte))
            {
                byte res;
                byte.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                sbyte res;
                sbyte.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(char))
            {
                char res;
                char.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(float))
            {
                float res;
                float.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(decimal))
            {
                decimal res;
                decimal.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(double))
            {
                double res;
                double.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(string))
            {
                result = (T)(object)vl;
            }
            else if (typeof(T) == typeof(System.DateTime))
            {
                long tick;
                var success = long.TryParse(vl, out tick);
                System.DateTime res = default;
                if (success)
                {
                    res = new System.DateTime(tick);
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(System.TimeSpan))
            {
                long tick;
                var success = long.TryParse(vl, out tick);
                System.TimeSpan res = default;
                if (success)
                {
                    res = new System.TimeSpan(tick);
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Bounds))
            {
                Bounds res = default;
                Bounds_Wrapper jsData = Util.GetDataFromJson<Bounds_Wrapper>(vl);
                if (jsData != null)
                {
                    res = new Bounds(jsData.center, jsData.size);
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(BoundsInt))
            {
                BoundsInt res = default;
                BoundsInt_Wrapper jsData = Util.GetDataFromJson<BoundsInt_Wrapper>(vl);
                if (jsData != null)
                {
                    res = new BoundsInt(jsData.position, jsData.size);
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Rect))
            {
                Rect res = default;
                Rect_Wrapper jsData = Util.GetDataFromJson<Rect_Wrapper>(vl);
                if (jsData != null)
                {
                    res = new Rect(jsData.position, jsData.size);
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(RectInt))
            {
                RectInt res = default;
                RectInt_Wrapper jsData = Util.GetDataFromJson<RectInt_Wrapper>(vl);
                if (jsData != null)
                {
                    res = new RectInt(jsData.position, jsData.size);
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Vector2Int))
            {
                Vector2Int res = default;
                Vector2Int_Wrapper jsData = Util.GetDataFromJson<Vector2Int_Wrapper>(vl);
                if (jsData != null)
                {
                    res = jsData.data;
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Vector3Int))
            {
                Vector3Int res = default;
                Vector3Int_Wrapper jsData = Util.GetDataFromJson<Vector3Int_Wrapper>(vl);
                if (jsData != null)
                {
                    res = jsData.data;
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Ray))
            {
                Ray res = default;
                Ray_Wrapper jsData = Util.GetDataFromJson<Ray_Wrapper>(vl);
                if (jsData != null)
                {
                    res = new Ray(jsData.rayOrigin, jsData.rayDirection);
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Ray2D))
            {
                Ray2D res = default;
                Ray2D_Wrapper jsData = Util.GetDataFromJson<Ray2D_Wrapper>(vl);
                if (jsData != null)
                {
                    res = new Ray2D(jsData.rayOrigin, jsData.rayDirection);
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(BoneWeight1))
            {
                BoneWeight1 res;
                res = Util.GetDataFromJson<BoneWeight1>(vl);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(BoneWeight))
            {
                BoneWeight res;
                res = Util.GetDataFromJson<BoneWeight>(vl);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Color))
            {
                Color res;
                res = Util.GetDataFromJson<Color>(vl);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Color32))
            {
                Color32 res;
                res = Util.GetDataFromJson<Color32>(vl);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(GradientAlphaKey))
            {
                GradientAlphaKey res;
                res = Util.GetDataFromJson<GradientAlphaKey>(vl);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(GradientColorKey))
            {
                GradientColorKey res;
                res = Util.GetDataFromJson<GradientColorKey>(vl);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(HumanPose))
            {
                HumanPose res;
                res = Util.GetDataFromJson<HumanPose>(vl);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(RangeInt))
            {
                RangeInt res;
                res = Util.GetDataFromJson<RangeInt>(vl);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Vector2))
            {
                Vector2 res;
                res = Util.GetDataFromJson<Vector2>(vl);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Vector3))
            {
                Vector3 res;
                res = Util.GetDataFromJson<Vector3>(vl);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Vector4))
            {
                Vector4 res;
                res = Util.GetDataFromJson<Vector4>(vl);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                Quaternion res;
                res = Util.GetDataFromJson<Quaternion>(vl);
                result = (T)(object)res;
            }
            return result;
        }

        internal static string ConvertAtomicValueToJsonString<T>(T v)
        {
            var usePettyPrint = Config.data == null ? false : Config.data.JsonPettyPrint;
            string result = "";
            if (typeof(T) == typeof(int) || typeof(T) == typeof(long) || typeof(T) == typeof(ulong) || typeof(T) == typeof(short)
                || typeof(T) == typeof(ushort) || typeof(T) == typeof(uint) || typeof(T) == typeof(bool) || typeof(T) == typeof(byte)
                || typeof(T) == typeof(sbyte) || typeof(T) == typeof(char) || typeof(T) == typeof(float) || typeof(T) == typeof(decimal)
                || typeof(T) == typeof(double) || typeof(T) == typeof(string))// || typeof(T) == typeof(System.Enum))
            {
                result = v.ToString();
            }
            else if (typeof(T) == typeof(System.DateTime))
            {
                System.DateTime curVal = (System.DateTime)(object)v;
                result = (curVal.Ticks).ToString();
            }
            else if (typeof(T) == typeof(System.TimeSpan))
            {
                System.TimeSpan curVal = (System.TimeSpan)(object)v;
                result = (curVal.Ticks).ToString();
            }
            else if (typeof(T) == typeof(Bounds))
            {
                Bounds curVal = (Bounds)(object)v;
                Bounds_Wrapper jsData = new Bounds_Wrapper { center = curVal.center, size = curVal.size };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(BoundsInt))
            {
                BoundsInt curVal = (BoundsInt)(object)v;
                BoundsInt_Wrapper jsData = new BoundsInt_Wrapper { position = curVal.position, size = curVal.size };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(Rect))
            {
                Rect curVal = (Rect)(object)v;
                Rect_Wrapper jsData = new Rect_Wrapper { position = curVal.position, size = curVal.size };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(RectInt))
            {
                RectInt curVal = (RectInt)(object)v;
                RectInt_Wrapper jsData = new RectInt_Wrapper { position = curVal.position, size = curVal.size };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(Vector2Int))
            {
                Vector2Int curVal = (Vector2Int)(object)v;
                Vector2Int_Wrapper jsData = new Vector2Int_Wrapper { data = curVal };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(Vector3Int))
            {
                Vector3Int curVal = (Vector3Int)(object)v;
                Vector3Int_Wrapper jsData = new Vector3Int_Wrapper { data = curVal };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(Ray))
            {
                Ray curVal = (Ray)(object)v;
                Ray_Wrapper jsData = new Ray_Wrapper { rayDirection = curVal.direction, rayOrigin = curVal.origin };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(Ray2D))
            {
                Ray2D curVal = (Ray2D)(object)v;
                Ray2D_Wrapper jsData = new Ray2D_Wrapper { rayDirection = curVal.direction, rayOrigin = curVal.origin };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(BoneWeight1) || typeof(T) == typeof(BoneWeight) || typeof(T) == typeof(Color)
                || typeof(T) == typeof(Color32) || typeof(T) == typeof(GradientAlphaKey) || typeof(T) == typeof(GradientColorKey)
                || typeof(T) == typeof(HumanPose) || typeof(T) == typeof(RangeInt) || typeof(T) == typeof(Vector2)
                || typeof(T) == typeof(Vector3) || typeof(T) == typeof(Vector4) || typeof(T) == typeof(Quaternion))
            {
                result = JsonUtility.ToJson(v, usePettyPrint);
            }
            else
            {
                throw new System.NotSupportedException("Data type not supported, " +
                    "Consider a feature request at: https://github.com/kaiyumcg/KSaveDataManager/issues");
            }
            return result;
        }
    }
}