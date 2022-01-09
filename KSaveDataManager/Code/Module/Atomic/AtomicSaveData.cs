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
            AtomicInternal.InsertAtomicIfReq(identifiers);
            var handle = new AtomicSaveData<T>();
            handle.keys = identifiers;
            return handle;
        }

        string[] keys;
        T g_value;

        public T Value
        {
            get
            {
                var st = AtomicInternal.GetAtomic(keys);
                g_value = GetValueFromString(st);
                return g_value;
            }
            set
            {
                var vl = value;
                var st = ConvertValueToString(vl);
                AtomicInternal.SetAtomic(st, keys);
                g_value = vl;
            }
        }

        //supports all well known simple data here such as int, float, datetime, vector etc
        T GetValueFromString(string vl)
        {
            return default;
        }

        string ConvertValueToString(T v)
        {
            return default;
        }
    }
}