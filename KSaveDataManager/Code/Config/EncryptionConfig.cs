using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
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
        public string AtomicPerDataSalt { get { return atomicPerDataSalt; }  set { atomicPerDataSalt = value; } }
        public string AtomicPerDataSharedSecret { get { return atomicPerDataSharedSecret; } set { atomicPerDataSharedSecret = value; } }
        public EncodingType AtomicPerDataCredentialEncoding { get { return atomicPerDataCredentialEncoding; } set { atomicPerDataCredentialEncoding = value; } }
        internal EncryptionConfig() { }
    }
}