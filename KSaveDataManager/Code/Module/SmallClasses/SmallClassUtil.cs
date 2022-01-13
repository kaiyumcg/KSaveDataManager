using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KSaveDataMan
{
    internal static class SmallClassUtil
    {
        internal static string GetClassSaveMasterFilePath()
        {
            var saveFileName = "classSaveMaster.sv";
            var saveDirectory = Application.persistentDataPath;
            if (Config.data != null && string.IsNullOrEmpty(Config.data.AtomicSaveFileName) == false)
            {
                saveFileName = Config.data.ClassSaveMasterFileName;
                saveDirectory = Config.data.SavePath == SaveFilePathMode.AppDataPath ? Application.dataPath : Application.persistentDataPath;
            }
            var fPath = Path.Combine(saveDirectory, saveFileName);
            return fPath;
        }
    }
}