using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    internal static class AtomUtil
    {
        internal static void CheckInternals<T>(string[] keys)
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
            AtomicInternal.InsertAtomicIfReq(keys, typeof(T).Name);
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
                if (AtomicInternal.setting.DebugMessage)
                {
                    Debug.LogWarning("Could not get data of type " + typeof(T) + " from json string. " +
                        "Exception is thrown! Err message: " + ex.Message);
                }
            }
            return result;
        }
    }
}