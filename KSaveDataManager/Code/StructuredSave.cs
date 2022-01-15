using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KSaveDataMan
{
    internal abstract class StructuredSave
    {
        protected abstract string GetMasterSaveFilePath();
        protected abstract void OnClear();
        protected abstract bool ShouldLocatorPointFileInDevice();
        protected abstract bool ShouldCreateLocatorIfNotFoundWhenLoadOperation();
        protected abstract bool ConsiderTypeNameForFindingProperLocatorWhenLoadOperation();
        protected abstract T OnLoadWithLocator<T>(ref LocatorForMasterSaveData locator, System.Action<LocalDataCode, T> OnComplete);
        protected abstract bool ConsiderTypeNameForFindingProperLocatorWhenSaveOperation();
        protected abstract void OnSaveWithLocator<T>(ref LocatorForMasterSaveData locator, T data, System.Action<LocalDataCode> OnComplete);

        protected MasterSaveJsonDefault masterSave = null;
        protected void EnsureMasterSave()
        {
            if (masterSave == null)
            {
                LoadMasterSaveToMemory();
                WriteMasterSaveToDevice();
            }
        }
        
        protected internal void LoadMasterSaveToMemory()
        {
            var fPath = GetMasterSaveFilePath();
            if (File.Exists(fPath))
            {
                var saveBytes = FileLoadUtil.LoadData(fPath);
                var masterSaveJson = CryptoUtil.DecryptIfRequired(saveBytes);
                masterSave = Util.GetDataFromJson<MasterSaveJsonDefault>(masterSaveJson);
            }
            else
            {
                masterSave = Util.GetDefaultMasterSave<MasterSaveJsonDefault>();
                masterSave.data = null;
            }
        }

        protected internal void WriteMasterSaveToDevice()
        {
            if (masterSave == null)
            {
                masterSave = Util.GetDefaultMasterSave<MasterSaveJsonDefault>();
                masterSave.data = null;
            }
            string json = Util.GetDataAsJsonString(masterSave);
            byte[] saveBytes = CryptoUtil.EncryptIfRequired(json);
            FileWriteUtil.WriteData(saveBytes, GetMasterSaveFilePath());
        }

        bool IsLocatorMatching(LocatorForMasterSaveData locator, string[] compKeys, string typeName, bool considerTypeName)
        {
            return Util.AreBothSame(compKeys, locator.keys, typeName, locator.typeName, considerTypeName);
        }

        void FindLocatorFor<T>(string[] keys, ref LocatorForMasterSaveData locator, bool considerTypeName = true)
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
        
        internal void Clear(string[] keys)
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
                        if (ShouldLocatorPointFileInDevice())
                        {
                            var fPath = data.value;
                            if (File.Exists(fPath))
                            {
                                File.Delete(fPath);
                            }
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
                masterSave = Util.GetDefaultMasterSave<MasterSaveJsonDefault>();
                masterSave.data = null;
            }
            else
            {
                masterSave.data = locators.ToArray();
            }
            OnClear();
            WriteMasterSaveToDevice();
        }

        string Create_A_LocatorValue(string[] keys)
        {
            var fName = Util.GetKeyedRandomName(keys) + Util.GetStructuredSaveFileExtension();
            var saveDir = Util.GetSaveDirectory();
            return Path.Combine(saveDir, fName);
        }

        string Create_A_LocatorValueForNoFile<T>()
        {
            var jsonValueOrPlainText = Util.ConvertAtomicValueToJsonString<T>(default);
            return CryptoUtil.EncryptStringAES(jsonValueOrPlainText);
        }

        internal T Load<T>(string[] keys, System.Action<LocalDataCode, T> OnComplete)
        {
            EnsureMasterSave();
            LocatorForMasterSaveData sel = null;
            FindLocatorFor<T>(keys, ref sel, ConsiderTypeNameForFindingProperLocatorWhenLoadOperation());
            if (sel == null)
            {
                if (ShouldCreateLocatorIfNotFoundWhenLoadOperation())
                {
                    var locatorValue = ShouldLocatorPointFileInDevice() ? Create_A_LocatorValue(keys) : Create_A_LocatorValueForNoFile<T>();
                    sel = new LocatorForMasterSaveData
                    {
                        value = locatorValue,
                        keys = Util.CPY(keys),
                        typeName = typeof(T).Name,
                        game_wrote_it = false
                    };
                    List<LocatorForMasterSaveData> currentLocators = new List<LocatorForMasterSaveData>();
                    currentLocators.AddRange(masterSave.data);
                    currentLocators.Add(sel);
                    masterSave.data = currentLocators.ToArray();
                    WriteMasterSaveToDevice();
                }
                OnComplete?.Invoke(LocalDataCode.NotFound, default);
                return default;
            }
            else
            {
                return OnLoadWithLocator(ref sel, OnComplete);
            }
        }

        internal void Save<T>(T data, string[] keys, System.Action<LocalDataCode> OnComplete)
        {
            EnsureMasterSave();
            LocatorForMasterSaveData sel = null;
            FindLocatorFor<T>(keys, ref sel, ConsiderTypeNameForFindingProperLocatorWhenSaveOperation());
            if (sel == null)
            {
                var locatorValue = ShouldLocatorPointFileInDevice() ? Create_A_LocatorValue(keys) : Create_A_LocatorValueForNoFile<T>();
                sel = new LocatorForMasterSaveData
                {
                    value = locatorValue,
                    keys = Util.CPY(keys),
                    typeName = typeof(T).Name,
                    game_wrote_it = true
                };
                List<LocatorForMasterSaveData> currentLocators = new List<LocatorForMasterSaveData>();
                currentLocators.AddRange(masterSave.data);
                currentLocators.Add(sel);
                masterSave.data = currentLocators.ToArray();
            }
            OnSaveWithLocator<T>(ref sel, data, OnComplete);
            WriteMasterSaveToDevice();
        }

        internal bool Exist<T>(string[] keys)
        {
            EnsureMasterSave();
            LocatorForMasterSaveData sel = null;
            FindLocatorFor<T>(keys, ref sel, ConsiderTypeNameForFindingProperLocatorWhenLoadOperation());
            return sel != null;
        }
    }
}