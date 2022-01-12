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
}