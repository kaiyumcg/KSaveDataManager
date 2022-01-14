using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSaveDataMan
{
    public enum SaveFilePathMode { PersistentDataPath = 0, AppDataPath = 1 }
    public enum LocalDataCode { Success = 0, PermissionOrIOError = 1, InvalidOperationError = 2 }
    public enum EncodingType { ASCII = 0, UTF_7 = 1, UTF_8 = 2, UTF_32 = 3, UNICODE = 4, BIG_ENDIAN = 5 }
    public enum CloudDataCode { Success = 0, NoInternet = 1, ServerConnectionError = 2, ServerStorageFull = 3, InvalidOperationError = 4 }
}