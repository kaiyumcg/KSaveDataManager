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

        //read mode simply update the data, write mode update the write flag as well as actual data inside
        //delete mode removes the internal representation of data pointed by keys
        //creation mode only works if the "search and find" actuall does not find any data in saveInternal
        internal static void SaveInternalOperation(ref AtomicSaves_Internal _saveInternal, ref string data, string[] keys,
            AtomicInternalOpMode mode, string typeName = "")
        {
            
        }

        internal static string GetAtomicSavePath()
        {
            var saveFileName = "atom.sv";
            var saveDirectory = Application.persistentDataPath;
            if (Config.data != null && string.IsNullOrEmpty(Config.data.AtomicSaveFileName) == false)
            {
                saveFileName = Config.data.AtomicSaveFileName;
                saveDirectory = Config.data._AtomicSavePath == AtomicSavePath.AppDataPath ? Application.dataPath : Application.persistentDataPath;
            }
            var fPath = Path.Combine(saveDirectory, saveFileName);
            return fPath;
        }

        internal static byte[] EncryptIfReq(string str, string key, string iv)
        {
            byte[] saveBytes = null;
            var useEncryption = false;
            if (Config.data != null && Config.data.EncryptionConfig != null)
            {
                useEncryption = true;
            }

            if (useEncryption)
            {
                saveBytes = CryptoUtil.EncryptStringToBytes(str, key, iv);
            }
            else
            {
                var encoding = EncodingType.ASCII;
                if (Config.data != null) { encoding = Config.data._EncodingType; }
                if (encoding == EncodingType.ASCII)
                {
                    saveBytes = Encoding.ASCII.GetBytes(str);
                }
                else if (encoding == EncodingType.BIG_ENDIAN)
                {
                    saveBytes = Encoding.BigEndianUnicode.GetBytes(str);
                }
                else if (encoding == EncodingType.UNICODE)
                {
                    saveBytes = Encoding.Unicode.GetBytes(str);
                }
                else if (encoding == EncodingType.UTF_32)
                {
                    saveBytes = Encoding.UTF32.GetBytes(str);
                }
                else if (encoding == EncodingType.UTF_7)
                {
                    saveBytes = Encoding.UTF7.GetBytes(str);
                }
                else if (encoding == EncodingType.UTF_8)
                {
                    saveBytes = Encoding.UTF8.GetBytes(str);
                }
            }
            return saveBytes;
        }

        internal static byte[] ConvertTo(string str)
        {
            byte[] result = null;
            var encoding = EncodingType.ASCII;
            if (Config.data != null) { encoding = Config.data._EncodingType; }
            if (encoding == EncodingType.ASCII)
            {
                result = Encoding.ASCII.GetBytes(str);
            }
            else if (encoding == EncodingType.BIG_ENDIAN)
            {
                result = Encoding.BigEndianUnicode.GetBytes(str);
            }
            else if (encoding == EncodingType.UNICODE)
            {
                result = Encoding.Unicode.GetBytes(str);
            }
            else if (encoding == EncodingType.UTF_32)
            {
                result = Encoding.UTF32.GetBytes(str);
            }
            else if (encoding == EncodingType.UTF_7)
            {
                result = Encoding.UTF7.GetBytes(str);
            }
            else if (encoding == EncodingType.UTF_8)
            {
                result = Encoding.UTF8.GetBytes(str);
            }
            else
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogError("Unknown encoding!");
                }
            }
            return result;
        }

        internal static string ConvertTo(byte[] data)
        {
            string result = null;
            var encoding = EncodingType.ASCII;
            if (Config.data != null) { encoding = Config.data._EncodingType; }
            if (encoding == EncodingType.ASCII)
            {
                result = Encoding.ASCII.GetString(data, 0, data.Length);
            }
            else if (encoding == EncodingType.BIG_ENDIAN)
            {
                result = Encoding.BigEndianUnicode.GetString(data, 0, data.Length);
            }
            else if (encoding == EncodingType.UNICODE)
            {
                result = Encoding.Unicode.GetString(data, 0, data.Length);
            }
            else if (encoding == EncodingType.UTF_32)
            {
                result = Encoding.UTF32.GetString(data, 0, data.Length);
            }
            else if (encoding == EncodingType.UTF_7)
            {
                result = Encoding.UTF7.GetString(data, 0, data.Length);
            }
            else if (encoding == EncodingType.UTF_8)
            {
                result = Encoding.UTF8.GetString(data, 0, data.Length);
            }
            else
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogError("Unknown encoding!");
                }
            }
            return result;
        }

        internal static string DecryptIfReq(byte[] data, string key, string iv)
        {
            string result = "";
            var useEncryption = false;
            if (Config.data != null && Config.data.EncryptionConfig != null)
            {
                useEncryption = true;
            }
            if (useEncryption)
            {
                result = CryptoUtil.DecryptStringFromBytes(data, key, iv);
            }
            else
            {
                var encoding = EncodingType.ASCII;
                if (Config.data != null) { encoding = Config.data._EncodingType; }
                if (encoding == EncodingType.ASCII)
                {
                    result = Encoding.ASCII.GetString(data, 0, data.Length);
                }
                else if (encoding == EncodingType.BIG_ENDIAN)
                {
                    result = Encoding.BigEndianUnicode.GetString(data, 0, data.Length);
                }
                else if (encoding == EncodingType.UNICODE)
                {
                    result = Encoding.Unicode.GetString(data, 0, data.Length);
                }
                else if (encoding == EncodingType.UTF_32)
                {
                    result = Encoding.UTF32.GetString(data, 0, data.Length);
                }
                else if (encoding == EncodingType.UTF_7)
                {
                    result = Encoding.UTF7.GetString(data, 0, data.Length);
                }
                else if (encoding == EncodingType.UTF_8)
                {
                    result = Encoding.UTF8.GetString(data, 0, data.Length);
                }
            }
            return result;
        }
    }
}