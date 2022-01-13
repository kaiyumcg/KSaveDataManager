using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    [System.Serializable]
    internal class ClassSaveMasterData
    {
        [SerializeField] internal string gameName = "";
        [SerializeField] internal string companyName = "";
        [SerializeField] internal string platform = "";
        [SerializeField] internal string buildGUID = "";
        [SerializeField] internal bool genuine = true;
        [SerializeField] internal string gameVer = "";
        [SerializeField] internal string unityVer = "";
        [SerializeField] internal string gameBundleID = "";
        [SerializeField] internal EncodingType encoding = EncodingType.ASCII;
        [SerializeField] internal ClassSaveJsonLocator[] data;
    }

    [System.Serializable]
    internal class ClassSaveJsonLocator
    {
        [SerializeField] internal string[] keys;
        [SerializeField] internal string json_file_path;
    }
}