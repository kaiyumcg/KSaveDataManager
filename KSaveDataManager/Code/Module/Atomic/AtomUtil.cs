using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace KSaveDataMan
{
    internal static class AtomUtil
    {
        static bool CheckInInternal(ref AtomicSaveMasterData save, string[] keys, ref AtomicSaveEachDataEntry intSave,
            bool deleteIfFound, bool createOneIfNotFound)
        {
            List<AtomicSaveEachDataEntry> intSvList = new List<AtomicSaveEachDataEntry>();
            if (deleteIfFound)
            {
                if (save != null && save.data != null && save.data.Length > 0)
                {
                    var data = save.data;
                    for (int i = 0; i < data.Length; i++)
                    {
                        intSvList.Add(data[i]);
                    }
                }
            }

            var exist = false;
            var len = keys == null ? 0 : keys.Length;
            if (save != null && save.data != null && save.data.Length > 0)
            {
                var data = save.data;
                for (int i = 0; i < data.Length; i++)
                {
                    var d = data[i];
                    if (d.keys == null) { continue; }
                    if (d.keys.Length != len) { continue; }
                    var match = true;
                    if (keys != null && keys.Length > 0)
                    {
                        for (int j = 0; j < keys.Length; j++)
                        {
                            var isThisAMatch = Array.Exists(d.keys, element => element == keys[j]);
                            if (isThisAMatch == false)
                            {
                                match = false;
                                break;
                            }
                        }
                    }

                    if (match)
                    {
                        exist = true;
                        intSave = data[i];
                        if (deleteIfFound)
                        {
                            intSvList.Remove(data[i]);
                        }
                        break;
                    }
                }
            }

            if (deleteIfFound)
            {
                save.data = intSvList.ToArray();
            }
            else if (createOneIfNotFound && exist == false)
            {

                var newSVString = new AtomicSaveEachDataEntry { game_wrote_it = false, keys = Util.CPY(keys), typeName = "", value = "" };
                intSvList.Add(newSVString);
                save.data = intSvList.ToArray();
            }

            return exist;
        }

        internal static void TryCreateIfNotExist(ref AtomicSaveMasterData saves_Internal, string[] keys, string typeName)
        {
            AtomicSaveEachDataEntry selSv = null;
            var exist = CheckInInternal(ref saves_Internal, keys, ref selSv, deleteIfFound: false, createOneIfNotFound: true);
        }

        internal static void TryDeleteIfExist(ref AtomicSaveMasterData saves_Internal, string[] keys)
        {
            AtomicSaveEachDataEntry selSv = null;
            var exist = CheckInInternal(ref saves_Internal, keys, ref selSv, deleteIfFound: true, createOneIfNotFound: false);
        }

        internal static void TryCheckIfDataExist(ref AtomicSaveMasterData saves_Internal, string[] keys, ref bool exist)
        {
            AtomicSaveEachDataEntry selSv = null;
            var existInInternal = CheckInInternal(ref saves_Internal, keys, ref selSv, deleteIfFound: false, createOneIfNotFound: false);
            if (existInInternal == false) { exist = false; }
            else if (existInInternal && selSv.game_wrote_it == false) { exist = false; }
            else { exist = true; }
        }

        internal static void TryGetData(ref AtomicSaveMasterData saves_Internal, string[] keys, ref string data)
        {
            AtomicSaveEachDataEntry selSv = null;
            var existInInternal = CheckInInternal(ref saves_Internal, keys, ref selSv, deleteIfFound: false, createOneIfNotFound: false);
            if (existInInternal) { data = selSv.value; }
            else { data = ""; }
        }

        internal static void TrySetData(ref AtomicSaveMasterData saves_Internal, string[] keys, string typeName, string data)
        {
            AtomicSaveEachDataEntry selSv = null;
            var existInInternal = CheckInInternal(ref saves_Internal, keys, ref selSv, deleteIfFound: false, createOneIfNotFound: false);
            if (existInInternal) { selSv.value = data; selSv.game_wrote_it = true; selSv.typeName = typeName; }
        }

        internal static string GetMasterSaveFilePath()
        {
            var saveFileName = "atom.sv";
            var saveDirectory = Application.persistentDataPath;
            if (Config.data != null && string.IsNullOrEmpty(Config.data.AtomicSaveFileName) == false)
            {
                saveFileName = Config.data.AtomicSaveFileName;
                saveDirectory = Config.data.SavePath == SaveFilePathMode.AppDataPath ? Application.dataPath : Application.persistentDataPath;
            }
            var fPath = Path.Combine(saveDirectory, saveFileName);
            return fPath;
        }

        internal static T GetValueFromString<T>(string vl) where T : struct
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
            else if (typeof(T) == typeof(System.Enum))
            {
                T res;
                System.Enum.TryParse<T>(vl, true, out res);
                result = (T)(object)res;
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
                    res = jsData.data;
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(BoundsInt))
            {
                BoundsInt res = default;
                BoundsInt_Wrapper jsData = Util.GetDataFromJson<BoundsInt_Wrapper>(vl);
                if (jsData != null)
                {
                    res = jsData.data;
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(LayerMask))
            {
                LayerMask res = default;
                LayerMask_Wrapper jsData = Util.GetDataFromJson<LayerMask_Wrapper>(vl);
                if (jsData != null)
                {
                    res = jsData.data;
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(Rect))
            {
                Rect res = default;
                Rect_Wrapper jsData = Util.GetDataFromJson<Rect_Wrapper>(vl);
                if (jsData != null)
                {
                    res = jsData.data;
                }
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(RectInt))
            {
                RectInt res = default;
                RectInt_Wrapper jsData = Util.GetDataFromJson<RectInt_Wrapper>(vl);
                if (jsData != null)
                {
                    res = jsData.data;
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

        internal static string ConvertValueToString<T>(T v) where T : struct
        {
            var usePettyPrint = Config.data == null ? false : Config.data.JsonPettyPrint;
            string result = "";
            if (typeof(T) == typeof(int) || typeof(T) == typeof(long) || typeof(T) == typeof(ulong) || typeof(T) == typeof(short)
                || typeof(T) == typeof(ushort) || typeof(T) == typeof(uint) || typeof(T) == typeof(bool) || typeof(T) == typeof(byte)
                || typeof(T) == typeof(sbyte) || typeof(T) == typeof(char) || typeof(T) == typeof(float) || typeof(T) == typeof(decimal)
                || typeof(T) == typeof(double) || typeof(T) == typeof(string) || typeof(T) == typeof(System.Enum))
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
                Bounds_Wrapper jsData = new Bounds_Wrapper { data = curVal };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(BoundsInt))
            {
                BoundsInt curVal = (BoundsInt)(object)v;
                BoundsInt_Wrapper jsData = new BoundsInt_Wrapper { data = curVal };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(LayerMask))
            {
                LayerMask curVal = (LayerMask)(object)v;
                LayerMask_Wrapper jsData = new LayerMask_Wrapper { data = curVal };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(Rect))
            {
                Rect curVal = (Rect)(object)v;
                Rect_Wrapper jsData = new Rect_Wrapper { data = curVal };
                result = JsonUtility.ToJson(jsData, usePettyPrint);
            }
            else if (typeof(T) == typeof(RectInt))
            {
                RectInt curVal = (RectInt)(object)v;
                RectInt_Wrapper jsData = new RectInt_Wrapper { data = curVal };
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