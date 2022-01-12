using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    public class EncryptionConfig
    {
        string _KEY = "AsISxq9OwdZag1163OJqwovXfSWG98m+sPjVwJecfe4=";
        string _IV = "Aq0UThtJhjbuyWXtmZs1rw==";
        public string KEY { get { return _KEY; } set { _KEY = value; } }
        public string IV { get { return _IV; } set { _IV = value; } }
    }
}