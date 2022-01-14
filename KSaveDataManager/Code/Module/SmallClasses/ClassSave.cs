using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KSaveDataMan
{
    internal static class ClassSave
    {
        static ClassSaveMasterData masterSave = null;
        static void EnsureMasterSave()
        {
            if (masterSave == null)
            {
                LoadMasterSaveToMemory();
                WriteMasterSaveToDevice();
            }
        }

        internal static void LoadMasterSaveToMemory()
        {
            var fPath = Util.GetMasterClassSaveFilePath();
            if (File.Exists(fPath))
            {
                var saveBytes = FileLoadUtil.LoadData(fPath);
                var masterSaveJson = CryptoUtil.DecryptIfRequired(saveBytes);
                masterSave = Util.GetDataFromJson<ClassSaveMasterData>(masterSaveJson);
            }
            else
            {
                masterSave = Util.GetDefaultMasterSave<ClassSaveMasterData>();
                masterSave.data = null;
            }
        }

        internal static void WriteMasterSaveToDevice()
        {
            if (masterSave == null)
            {
                masterSave = Util.GetDefaultMasterSave<ClassSaveMasterData>();
                masterSave.data = null;
            }
            string json = Util.GetDataAsJsonString(masterSave);
            byte[] saveBytes = CryptoUtil.EncryptIfRequired(json);
            var fPath = Util.GetMasterClassSaveFilePath();
            FileWriteUtil.WriteData(saveBytes, fPath);
        }

        static bool IsLocatorMatching(LocatorForMasterSaveData locator, string[] compKeys, string typeName, bool considerTypeName)
        {
            return Util.AreBothSame(compKeys, locator.keys, typeName, locator.typeName, considerTypeName);
        }

        static void FindLocatorFor<T>(string[] keys, ref LocatorForMasterSaveData locator, bool considerTypeName = true)
        {
            locator = null;
            if (masterSave != null && masterSave.data != null && masterSave.data.Length > 0)
            {
                for (int i = 0; i < masterSave.data.Length; i++)
                {
                    var data = masterSave.data[i];
                    if (data == null) { continue; }
                    var fetch = IsLocatorMatching(data, keys, typeof(T).Name, considerTypeName);
                    if (fetch)
                    {
                        locator = data;
                        break;
                    }
                }
            }
        }

        internal static void Clear(string[] keys)
        {
            EnsureMasterSave();
            List<LocatorForMasterSaveData> locators = new List<LocatorForMasterSaveData>();
            var keyLen = keys == null ? 0 : keys.Length;
            var allClean = keys == null || keyLen == 0;
            if (masterSave != null && masterSave.data != null && masterSave.data.Length > 0)
            {
                for (int i = 0; i < masterSave.data.Length; i++)
                {
                    var data = masterSave.data[i];
                    if (data == null) { continue; }
                    var deleteIt = allClean ? true : IsLocatorMatching(data, keys, "", considerTypeName: false);
                    if (deleteIt)
                    {
                        var fPath = data.value;
                        if (File.Exists(fPath))
                        {
                            File.Delete(fPath);
                        }
                    }
                    else
                    {
                        locators.Add(data);
                    }
                }
            }

            if (allClean)
            {
                masterSave = Util.GetDefaultMasterSave<ClassSaveMasterData>();
                masterSave.data = null;
            }
            else
            {
                masterSave.data = locators.ToArray();
            }
            WriteMasterSaveToDevice();
        }

        internal static void Load<T>(string[] keys, System.Action<LocalDataCode, T> OnComplete)
        {
            EnsureMasterSave();
            LocatorForMasterSaveData sel = null;
            FindLocatorFor<T>(keys, ref sel);
            if (sel == null)
            {
                OnComplete?.Invoke(LocalDataCode.NotFound, default);
            }
            else
            {
                var fPath = sel.value;
                FileLoadUtil.LoadDataAsync(fPath, (success, jSave) =>
                {
                    if (success)
                    {
                        var json = CryptoUtil.DecryptIfRequired(jSave);
                        var result = Util.GetDataFromJson<T>(json);
                        OnComplete?.Invoke(result == null ? LocalDataCode.ConversionError : LocalDataCode.Success, result);
                    }
                    else
                    {
                        OnComplete?.Invoke(LocalDataCode.DiskLoadError, default);
                    }
                });
            }
        }

        internal static void Save<T>(T data, string[] keys, System.Action<LocalDataCode> OnComplete)
        {
            EnsureMasterSave();
            LocatorForMasterSaveData sel = null;
            FindLocatorFor<T>(keys, ref sel);
            if (sel == null)
            {
                var fName = Util.GetKeyedRandomName(keys) + ".SVJ";
                var saveDir = Util.GetSaveDirectory();
                sel = new LocatorForMasterSaveData
                {
                    value = Path.Combine(saveDir, fName),
                    keys = Util.CPY(keys),
                    typeName = typeof(T).Name
                };
                List<LocatorForMasterSaveData> currentLocators = new List<LocatorForMasterSaveData>();
                currentLocators.AddRange(masterSave.data);
                currentLocators.Add(sel);
                masterSave.data = currentLocators.ToArray();
            }

            var json = Util.GetDataAsJsonString(data);
            var wBytes = CryptoUtil.EncryptIfRequired(json);
            FileWriteUtil.WriteDataAsync(wBytes, sel.value, (success) =>
            {
                OnComplete?.Invoke(success ? LocalDataCode.Success : LocalDataCode.DiskWriteError);
            });
            WriteMasterSaveToDevice();
        }
    }
}