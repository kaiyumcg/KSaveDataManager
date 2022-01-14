using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    [System.Serializable]
    internal class ClassSaveMasterData : MasterSaveJsonDefault
    {
        [SerializeField] internal LocatorForMasterSaveData[] data = null;
    }
}