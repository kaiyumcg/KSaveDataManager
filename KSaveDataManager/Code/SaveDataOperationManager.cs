using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    internal class SaveDataOperationManager : MonoBehaviour
    {
        static SaveDataOperationManager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                if (instance != this)
                {
                    Destroy(this);
                }
            }
        }

        private void OnDestroy()
        {
            AtomicSaveInternalController.WriteMasterSave();
            ClassSave.WriteMasterSave();
        }

        private void OnDisable()
        {
            AtomicSaveInternalController.WriteMasterSave();
            ClassSave.WriteMasterSave();
        }
    }
}