using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace KSaveDataMan
{
    public static class BigDataModule
    {
        public static void LoadAllBytesAsync(string identifier, OnReadComplete callback = null)
        {

        }

        //we should compute save path ourself from certain user friendly folder or url or enum mode
        public static void WriteAllBytesAsync(byte[] data, string savePath, OnWriteComplete callback = null)
        {
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
            //StartCoroutine(NowCheckForResourceUnload(savePath));//todo, this line must not 
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
                else
                {
                    throw new Exception("could not find the flag in the pool!");
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

        static Dictionary<string, byte[]> dataPool = new Dictionary<string, byte[]>();
        static Dictionary<string, Thread> threadPool = new Dictionary<string, Thread>();
        static Dictionary<string, bool> completedFlagPool = new Dictionary<string, bool>();
        static Dictionary<string, OnWriteComplete> callbackPool = new Dictionary<string, OnWriteComplete>();

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
                throw new Exception("could not find the data to write on dataPool!");
            }
            File.WriteAllBytes(savePath, data);
            Debug.LogWarning("data saved!- used threading for path: " + savePath);
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