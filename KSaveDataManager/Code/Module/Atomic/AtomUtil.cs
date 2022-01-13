using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace KSaveDataMan
{
    internal static class AtomUtil
    {
        static bool CheckInInternal(ref AtomicSaveMasterData save, string[] keys, ref AtomicSaveEachDataEntry intSave,
            bool deleteIfFound, bool createOneIfNotFound)
        {
            List<AtomicSaveEachDataEntry> intSvList = new List<AtomicSaveEachDataEntry>();
            if (deleteIfFound)
            {
                if (save != null && save.data != null && save.data.Length > 0)
                {
                    var data = save.data;
                    for (int i = 0; i < data.Length; i++)
                    {
                        intSvList.Add(data[i]);
                    }
                }
            }

            var exist = false;
            var len = keys == null ? 0 : keys.Length;
            if (save != null && save.data != null && save.data.Length > 0)
            {
                var data = save.data;
                for (int i = 0; i < data.Length; i++)
                {
                    var d = data[i];
                    if (d.keys == null) { continue; }
                    if (d.keys.Length != len) { continue; }
                    var match = true;
                    if (keys != null && keys.Length > 0)
                    {
                        for (int j = 0; j < keys.Length; j++)
                        {
                            var isThisAMatch = Array.Exists(d.keys, element => element == keys[j]);
                            if (isThisAMatch == false)
                            {
                                match = false;
                                break;
                            }
                        }
                    }

                    if (match)
                    {
                        exist = true;
                        intSave = data[i];
                        if (deleteIfFound)
                        {
                            intSvList.Remove(data[i]);
                        }
                        break;
                    }
                }
            }

            if (deleteIfFound)
            {
                save.data = intSvList.ToArray();
            }
            else if (createOneIfNotFound && exist == false)
            {

                var newSVString = new AtomicSaveEachDataEntry { game_wrote_it = false, keys = Util.CPY(keys), typeName = "", value = "" };
                intSvList.Add(newSVString);
                save.data = intSvList.ToArray();
            }

            return exist;
        }

        internal static void TryCreateIfNotExist(ref AtomicSaveMasterData saves_Internal, string[] keys, string typeName)
        {
            AtomicSaveEachDataEntry selSv = null;
            var exist = CheckInInternal(ref saves_Internal, keys, ref selSv, deleteIfFound: false, createOneIfNotFound: true);
        }

        internal static void TryDeleteIfExist(ref AtomicSaveMasterData saves_Internal, string[] keys)
        {
            AtomicSaveEachDataEntry selSv = null;
            var exist = CheckInInternal(ref saves_Internal, keys, ref selSv, deleteIfFound: true, createOneIfNotFound: false);
        }

        internal static void TryCheckIfDataExist(ref AtomicSaveMasterData saves_Internal, string[] keys, ref bool exist)
        {
            AtomicSaveEachDataEntry selSv = null;
            var existInInternal = CheckInInternal(ref saves_Internal, keys, ref selSv, deleteIfFound: false, createOneIfNotFound: false);
            if (existInInternal == false) { exist = false; }
            else if (existInInternal && selSv.game_wrote_it == false) { exist = false; }
            else { exist = true; }
        }

        internal static void TryGetData(ref AtomicSaveMasterData saves_Internal, string[] keys, ref string data)
        {
            AtomicSaveEachDataEntry selSv = null;
            var existInInternal = CheckInInternal(ref saves_Internal, keys, ref selSv, deleteIfFound: false, createOneIfNotFound: false);
            if (existInInternal) { data = selSv.value; }
            else { data = ""; }
        }

        internal static void TrySetData(ref AtomicSaveMasterData saves_Internal, string[] keys, string typeName, string data)
        {
            AtomicSaveEachDataEntry selSv = null;
            var existInInternal = CheckInInternal(ref saves_Internal, keys, ref selSv, deleteIfFound: false, createOneIfNotFound: false);
            if (existInInternal) { selSv.value = data; selSv.game_wrote_it = true; selSv.typeName = typeName; }
        }

        internal static string GetMasterAtomicSaveFilePath()
        {
            var saveFileName = "atom.sv";
            var saveDirectory = Application.persistentDataPath;
            if (Config.data != null && string.IsNullOrEmpty(Config.data.AtomicSaveFileName) == false)
            {
                saveFileName = Config.data.AtomicSaveFileName;
                saveDirectory = Config.data.SavePath == SaveFilePathMode.AppDataPath ? Application.dataPath : Application.persistentDataPath;
            }
            var fPath = Path.Combine(saveDirectory, saveFileName);
            return fPath;
        }
    }
}