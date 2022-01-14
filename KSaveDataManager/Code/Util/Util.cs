using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    internal static class Util 
    {
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

        internal static string[] CPY(string[] input)
        {
            var result = new string[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = input[i];
            }
            return result;
        }
    }
}