using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    [System.Serializable]
    internal class AtomicSaveString_Internal
    {
        [SerializeField] internal string[] keys;
        [SerializeField] internal string value;
        [SerializeField] internal bool game_wrote_it;
        [SerializeField] internal string typeName;
    }

    [System.Serializable]
    internal class AtomicSaves_Internal
    {
        [SerializeField] internal AtomicSaveString_Internal[] data;
    }

    //depending upon user main setting, we might choose to do it with playerpref as well
    internal static class AtomicInternal
    {
        internal static AtomicSaves_Internal saveInternal = null;
        internal static SaveDataOperationManager opMan = null;
        internal static SaveDataSetting setting = null;
        internal static EncryptionUsageDescription encryptionUsage = null;

        internal static void WriteToDevice()
        {
            //convert 'saveInternal' to json string
            //encrypt the json string or not depending upon setting
            //write the string as byte array to "atomic.sv" file in persistent data folder
        }

        internal static void LoadFromDevice()
        {
            //load "atomic.sv" from persistent data folder into string
            //decrypt the json or not depending upon setting
            //convert json to object and store it inside 'saveInternal'
        }

        internal static void DeleteData(string[] keys)
        {
            if (keys == null)
            {
                //totally flush atomic structure
            }
            else
            {
                //search through entire data and delete it
            }
        }

        internal static void InsertAtomicIfReq(string[] keys, string typeName)
        {

        }

        internal static string GetAtomic(string[] keys, string typeName)
        {
            throw new System.NotImplementedException();
        }

        internal static void SetAtomic(string data, string[] keys, string typeName)
        {
            //set wrote flag
            throw new System.NotImplementedException();
        }

        internal static bool HasData(string[] keys, string typeName)
        {
            throw new System.NotImplementedException();
        }
    }
}