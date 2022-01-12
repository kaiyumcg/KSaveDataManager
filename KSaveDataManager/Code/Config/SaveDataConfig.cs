using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    public class SaveDataConfig
    {
        bool debugMessage = false;
        bool jsonPettyPrint = false;
        bool useUnityPlayerPrefForAtomic = false;
        EncryptionConfig encryptionConfig = null;
        CloudConfig cloudConfig = null;
        public bool DebugMessage { get { return debugMessage; } set { debugMessage = value; } }
        public bool JsonPettyPrint { get { return jsonPettyPrint; } set { jsonPettyPrint = value; } }
        public bool UseUnityPlayerPrefForAtomic { get { return useUnityPlayerPrefForAtomic; } set { useUnityPlayerPrefForAtomic = value; } }
        public EncryptionConfig EncryptionConfig { get { return encryptionConfig; } set { encryptionConfig = value; } }
        public CloudConfig CloudConfig { get { return cloudConfig; } set { cloudConfig = value; } }
    }
}