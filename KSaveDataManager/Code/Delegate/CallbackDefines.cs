using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    public delegate void OnWriteComplete(bool success);
    public delegate void OnReadComplete(bool success, byte[] data);
}