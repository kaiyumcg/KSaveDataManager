using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    internal static class Config
    {
        internal static SaveDataConfig data = null;
    }

    public class EncryptionConfig
    {
        string _KEY = "AsISxq9OwdZag1163OJqwovXfSWG98m+sPjVwJecfe4=";
        string _IV = "Aq0UThtJhjbuyWXtmZs1rw==";
        bool encryptEachData = false;
        string atomicPerDataSalt = "o6806642kbM7c5";
        string atomicPerDataSharedSecret = "o7x8y6";
        EncodingType atomicPerDataCredentialEncoding = EncodingType.ASCII;
        public string KEY { get { return _KEY; } set { _KEY = value; } }
        public string IV { get { return _IV; } set { _IV = value; } }
        public bool AtomicEncryptEachData { get { return encryptEachData; } set { encryptEachData = value; } }
        public string AtomicPerDataSalt { get { return atomicPerDataSalt; } set { atomicPerDataSalt = value; } }
        public string AtomicPerDataSharedSecret { get { return atomicPerDataSharedSecret; } set { atomicPerDataSharedSecret = value; } }
        public EncodingType AtomicPerDataCredentialEncoding { get { return atomicPerDataCredentialEncoding; } set { atomicPerDataCredentialEncoding = value; } }
        
        internal EncryptionConfig() { }
    }

    public class CloudConfig
    {
        internal CloudConfig() { }
    }

    public class SaveDataConfig
    {
        string structuredSaveExtension = ".SVST";
        bool debugMessage = false;
        bool jsonPettyPrint = false;
        EncryptionConfig encryptionConfig = null;
        CloudConfig cloudConfig = null;
        EncodingType encodingType = EncodingType.ASCII;
        string atomicSaveFileName = "atom.sv";
        string classSaveMasterFileName = "classSaveMaster.sv";
        string bigDataSaveMasterFileName = "bigData.sv";
#if UNITY_EDITOR
        SaveFilePathMode savePath = SaveFilePathMode.AppDataPath;
#else
        SaveFilePathMode savePath = SaveFilePathMode.PersistentDataPath;
#endif
        public bool DebugMessage { get { return debugMessage; } set { debugMessage = value; } }
        public bool JsonPettyPrint { get { return jsonPettyPrint; } set { jsonPettyPrint = value; } }
        public EncryptionConfig EncryptionConfig { get { return encryptionConfig; } set { encryptionConfig = value; } }
        public CloudConfig CloudConfig { get { return cloudConfig; } set { cloudConfig = value; } }
        public EncodingType _EncodingType { get { return encodingType; } set { encodingType = value; } }
        public SaveFilePathMode SavePath { get { return savePath; } set { savePath = value; } }
        public string AtomicMasterSaveFileName { get { return atomicSaveFileName; } set { atomicSaveFileName = value; } }
        public string ClassSaveMasterFileName { get { return classSaveMasterFileName; } set { classSaveMasterFileName = value; } }
        public string BigDataSaveMasterFileName { get { return bigDataSaveMasterFileName; } set { bigDataSaveMasterFileName = value; } }
        public string StructuredSaveExtension { get { return structuredSaveExtension; } set { structuredSaveExtension = value; } }
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