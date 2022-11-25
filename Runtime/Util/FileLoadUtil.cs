using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace KSaveDataMan
{
    internal static class FileLoadUtil
    {
        internal static byte[] LoadData(string fPath)
        {
            byte[] result = null;
            try
            {
                result = File.ReadAllBytes(fPath);
            }
            catch (System.Exception ex)
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogError("Can not load file from disk to memory! Exception message: " + ex.Message);
                }
            }
            return result;
        }

        internal static void LoadDataAsync(string loadPath, OnReadComplete onComplete)
        {
            SaveDataManager.operationManager.StartCoroutine(LoadFileCOR(loadPath, onComplete));
            //SaveDataManager.operationManager.StartCoroutine(LoadFile2COR(loadPath, onComplete));
        }

        static IEnumerator LoadFileCOR(string loadPath, OnReadComplete onComplete)
        {
            WWW www = new WWW("file://" + loadPath);
            yield return www;
            byte[] result = null;
            var success = false;
            if (!string.IsNullOrEmpty(www.error))
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogError("Error while loading file : " + www.error);
                }
            }
            else
            {
                result = www.bytes;
                success = true;
            }
            www.Dispose();
            onComplete?.Invoke(success, result);
        }

        static IEnumerator LoadFile2COR(string loadPath, OnReadComplete onComplete)
        {
            byte[] result = null;
            var success = false;
            using (UnityWebRequest webRequest = UnityWebRequest.Get("file://" + loadPath))
            {
                yield return webRequest.SendWebRequest();
                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    if (Config.data != null && Config.data.DebugMessage)
                    {
                        Debug.LogError("Error while loading file : " + webRequest.error);
                    }
                }
                else
                {
                    result = webRequest.downloadHandler.data;
                    success = true;
                }
            }
            onComplete?.Invoke(success, result);
        }
    }
}