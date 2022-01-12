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
        bool useDifferentCredentialForEachKeyEncryption = false;
        string _KEY_PER_DATA = "AsISxq9OwdZag1163OJqwovXfSWG98m+sPjVwJecfe4=";
        string _IV_PER_DATA = "Aq0UThtJhjbuyWXtmZs1rw==";
        public string KEY { get { return _KEY; } set { _KEY = value; } }
        public string IV { get { return _IV; } set { _IV = value; } }
        public bool EncryptEachData { get { return encryptEachData; } set { encryptEachData = value; } }
        public bool UseDifferentCredentialForEachDataEncryption 
        { get { return useDifferentCredentialForEachKeyEncryption; } set { useDifferentCredentialForEachKeyEncryption = value; } }
        public string PerDataKey { get { return _KEY_PER_DATA; }  set { _KEY_PER_DATA = value; } }
        public string PerDataIV { get { return _IV_PER_DATA; } set { _IV_PER_DATA = value; } }
        internal EncryptionConfig() { }
    }
}