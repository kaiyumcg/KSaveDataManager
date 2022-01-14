using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    [System.Serializable]
    internal class MasterSaveJsonDefault
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
    }

    [System.Serializable]
    internal class LocatorForMasterSaveData
    {
        [SerializeField] internal string[] keys;
        [SerializeField] internal string value;
        [SerializeField] internal string typeName;
    }
}