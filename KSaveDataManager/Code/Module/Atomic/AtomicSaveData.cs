using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    public class AtomicSaveData<T> where T : struct
    {
        string[] keys = null;
        AtomicSave save = null;

        private AtomicSaveData() { }
        internal static AtomicSaveData<T> CreateHandle(string[] identifiers)
        {
            var handle = new AtomicSaveData<T>();
            handle.keys = identifiers;
            handle.save = SaveDataManager.AtomicSave;
            return handle;
        }

        public bool HasData()
        {
            return save.Exist<T>(keys);
        }

        public T Value
        {
            get
            {
                return save.Load<T>(keys, null); ;
            }
            set
            {
                save.Save<T>(value, keys, null);
            }
        }
    }
}