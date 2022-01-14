using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    public class AtomicSaveData<T> where T : struct
    {
        string[] keys = null;
        T g_value = default;

        private AtomicSaveData() { }
        internal static AtomicSaveData<T> CreateHandle(string[] identifiers)
        {
            AtomicSaveInternalController.Check<T>(identifiers);
            var handle = new AtomicSaveData<T>();
            handle.keys = identifiers;
            return handle;
        }

        public bool HasData()
        {
            return AtomicSaveInternalController.HasData<T>(keys);
        }

        public T Value
        {
            get
            {
                var st = AtomicSaveInternalController.GetAtomic<T>(keys);
                g_value = AtomUtil.GetValueFromString<T>(st);
                return g_value;
            }
            set
            {
                var vl = value;
                var st = AtomUtil.ConvertValueToString<T>(vl);
                AtomicSaveInternalController.SetAtomic<T>(st, keys);
                g_value = vl;
            }
        }

        
    }
}