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
        EncodingType encodingType = EncodingType.ASCII;
        string atomicSaveFileName = "atom.sv";
#if UNITY_EDITOR
        AtomicSavePath atomicSavePath = AtomicSavePath.AppDataPath;
#else
        AtomicSavePath atomicSavePath = AtomicSavePath.PersistentDataPath;
#endif
        public bool DebugMessage { get { return debugMessage; } set { debugMessage = value; } }
        public bool JsonPettyPrint { get { return jsonPettyPrint; } set { jsonPettyPrint = value; } }
        public bool UseUnityPlayerPrefForAtomic { get { return useUnityPlayerPrefForAtomic; } set { useUnityPlayerPrefForAtomic = value; } }
        public EncryptionConfig EncryptionConfig { get { return encryptionConfig; } set { encryptionConfig = value; } }
        public CloudConfig CloudConfig { get { return cloudConfig; } set { cloudConfig = value; } }
        public EncodingType _EncodingType { get { return encodingType; } set { encodingType = value; } }
        public AtomicSavePath _AtomicSavePath { get { return atomicSavePath; } set { atomicSavePath = value; } }
        public string AtomicSaveFileName { get { return atomicSaveFileName; } set { atomicSaveFileName = value; } }

        public static EncryptionConfig CreateEncryptionConfig()
        {
            return new EncryptionConfig();
        }

        public static CloudConfig CreateCloudConfig()
        {
            return new CloudConfig();
        }
    }
}