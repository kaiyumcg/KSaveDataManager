using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace KSaveDataMan
{
    internal static class FileWriteUtil
    {
        static bool useDebug = false;
        static Dictionary<string, byte[]> dataPool = new Dictionary<string, byte[]>();
        static Dictionary<string, Thread> threadPool = new Dictionary<string, Thread>();
        static Dictionary<string, bool> completedFlagPool = new Dictionary<string, bool>();
        static Dictionary<string, OnWriteComplete> callbackPool = new Dictionary<string, OnWriteComplete>();

        internal static void WriteData(byte[] data, string fPath)
        {
            try
            {
                File.WriteAllBytes(fPath, data);
            }
            catch (System.Exception ex)
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogError("Can not write file into disk! Exception message: " + ex.Message);
                }
            }
        }

        internal static void WriteDataAsync(byte[] data, string savePath, OnWriteComplete callback = null)
        {
            useDebug = Config.data == null ? true : Config.data.DebugMessage;
            if (dataPool.ContainsKey(savePath))
            {
                dataPool[savePath] = data;
            }
            else
            {
                dataPool.Add(savePath, data);
            }

            SaveDataThreaded(savePath);

            if (completedFlagPool.ContainsKey(savePath))
            {
                completedFlagPool[savePath] = false;
            }
            else
            {
                completedFlagPool.Add(savePath, false);
            }

            if (callbackPool.ContainsKey(savePath))
            {
                callbackPool[savePath] = callback;
            }
            else
            {
                callbackPool.Add(savePath, callback);
            }
            SaveDataManager.operationManager.StartCoroutine(NowCheckForResourceUnload(savePath));
        }

        static IEnumerator NowCheckForResourceUnload(string savePath)
        {
            yield return null;
            while (true)
            {
                bool flag = false;
                if (completedFlagPool.ContainsKey(savePath))
                {
                    flag = completedFlagPool[savePath];
                }
                if (flag)
                {
                    break;
                }
                yield return null;
            }
            Resources.UnloadUnusedAssets();
            if (completedFlagPool.ContainsKey(savePath))
            {
                completedFlagPool.Remove(savePath);
            }

            if (completedFlagPool.ContainsKey(savePath))
            {
                completedFlagPool.Remove(savePath);
            }
        }

        static void SaveDataThreaded(string savePath)
        {
            Thread thread = new Thread(() => SaveDataTaskThreaded(savePath));
            if (threadPool.ContainsKey(savePath))
            {
                threadPool[savePath] = thread;
            }
            else
            {
                threadPool.Add(savePath, thread);
            }
            thread.Start();
        }

        static void SaveDataTaskThreaded(string savePath)
        {
            byte[] data = null;
            if (dataPool.ContainsKey(savePath))
            {
                data = dataPool[savePath];
            }
            else
            {
                CallCB(savePath, false);
                if (useDebug)
                {
                    throw new Exception("could not find the data to write on dataPool! Async file write error!");
                }
            }
            File.WriteAllBytes(savePath, data);
            if (useDebug)
            {
                Debug.LogWarning("data saved!- used threading for path: " + savePath);
            }
            
            data = null;
            CallCB(savePath, true);
            if (dataPool.ContainsKey(savePath))
            {
                dataPool.Remove(savePath);
            }
            if (threadPool.ContainsKey(savePath))
            {
                threadPool.Remove(savePath);
            }

            if (callbackPool.ContainsKey(savePath))
            {
                callbackPool.Remove(savePath);
            }

            completedFlagPool[savePath] = true;
        }

        static void CallCB(string savePath, bool flag)
        {
            OnWriteComplete cb = null;
            if (callbackPool.ContainsKey(savePath))
            {
                cb = callbackPool[savePath];
            }

            if (cb != null)
            {
                cb(flag);
            }
        }
    }
}