using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    public class AtomicSaveData<T> where T : struct
    {
        private AtomicSaveData() { }
        internal static AtomicSaveData<T> CreateHandle(string[] identifiers)
        {
            if (AtomicInternal.opMan == null)
            {
                var g = new GameObject("_SaveDataOperationManager_Gen_");
                AtomicInternal.opMan = g.AddComponent<SaveDataOperationManager>();
            }

            if (AtomicInternal.saveInternal == null)
            {
                AtomicInternal.LoadFromDevice();
            }
            AtomicInternal.InsertAtomicIfReq(identifiers, typeof(T).Name);
            var handle = new AtomicSaveData<T>();
            handle.keys = identifiers;
            return handle;
        }

        string[] keys;
        T g_value;

        public bool HasData()
        {
            return AtomicInternal.HasData(keys, typeof(T).Name);
        }

        public T Value
        {
            get
            {
                var st = AtomicInternal.GetAtomic(keys, typeof(T).Name);
                g_value = GetValueFromString(st);
                return g_value;
            }
            set
            {
                var vl = value;
                var st = ConvertValueToString(vl);
                AtomicInternal.SetAtomic(st, keys, typeof(T).Name);
                g_value = vl;
            }
        }

        T GetValueFromString(string vl)
        {
            T result = default;
            if (typeof(T) == typeof(int))
            {
                int res = default;
                int.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(long))
            {
                long res = default;
                long.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(ulong))
            {
                ulong res = default;
                ulong.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(short))
            {
                short res = default;
                short.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(ushort))
            {
                ushort res = default;
                ushort.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(uint))
            {
                uint res = default;
                uint.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(bool))
            {
                bool res = default;
                bool.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(byte))
            {
                byte res = default;
                byte.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                sbyte res = default;
                sbyte.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(char))
            {
                char res = default;
                char.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(float))
            {
                float res = default;
                float.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(decimal))
            {
                decimal res = default;
                decimal.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(double))
            {
                double res = default;
                double.TryParse(vl, out res);
                result = (T)(object)res;
            }
            else if (typeof(T) == typeof(string))
            {
                result = (T)(object)vl;
            }
            else if (typeof(T) == typeof(System.Enum))
            {
                T res = default;
                System.Enum.TryParse<T>(vl, true, out res);
                result = (T)(object)res;
            }

            //Bounds, BoundsInt, BoneWeight1, BoneWeight, Color, Color32, Gradient, GradientAlphaKey, GradientColorKey, 
            //Grid, GridLayout, Hash128, HumanBone, HumanBodyBone, HumanPose, Keyframe, LayerMask, SortingLayer, Light,
            //Material, Motion, Ray, RangeInt, Ray2D, RaycastHit, RaycastHit2D, Rect, RectInt, Vector2, Vector2Int, Vector3, Vector3Int, Vector4
            //DateTime
            //TimeSpan

            //GameObject, seperate handler through json


            //DONE
            //wrapper class
            //Bounds, BoundsInt, LayerMask, Rect, RectInt, Vector2Int, Vector3Int

            //total custom class
            //Ray, Ray2D, DateTime, TimeSpan

            //direct json
            //BoneWeight1, BoneWeight, Color, Color32, GradientAlphaKey, GradientColorKey, HumanPose, RangeInt, Vector2, Vector3, Vector4, Quaternion

            return default;
        }

        string ConvertValueToString(T v)
        {
            return default;
        }
    }
}