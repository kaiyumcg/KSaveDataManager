using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

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
            if (setting.UseUnityPlayerPrefForAtomic) { PlayerPrefs.Save(); }
            else
            {
                if (saveInternal == null) { return; }
                string json = "";
                try
                {
                    json = JsonUtility.ToJson(saveInternal);
                }
                catch (System.Exception ex)
                {
                    if (setting.DebugMessage) 
                    { 
                        Debug.LogError("Can not convert the save data into json. Exception message: " + ex.Message); 
                    }
                }
                byte[] saveBytes = null;
                try
                {
                    saveBytes = encryptionUsage == null ? Encoding.ASCII.GetBytes(json) : CryptoUtil.EncryptStringToBytes(json);
                }
                catch (System.Exception ex)
                {
                    if (setting.DebugMessage)
                    {
                        Debug.LogError("Byte conversion error. This is most likely due to invalid encryption operation. Exception message: " + ex.Message);
                    }
                }
                
                var fPath = Path.Combine(Application.persistentDataPath, "atomic.sv");
                try
                {
                    File.WriteAllBytes(fPath, saveBytes);
                }
                catch (System.Exception ex)
                {
                    if (setting.DebugMessage)
                    {
                        Debug.LogError("Can not write the atomic save file into disk! Exception message: " + ex.Message);
                    }
                }
            }
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

        //create set with keys if not exist
        internal static void InsertAtomicIfReq(string[] keys, string typeName)
        {

        }

        //playerpref get or get from our data
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