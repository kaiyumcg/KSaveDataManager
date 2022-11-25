using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KSaveDataMan
{
    internal class ClassSave : StructuredSave
    {
        protected override bool ConsiderTypeNameForFindingProperLocatorWhenLoadOperation()
        {
            return true;
        }

        protected override bool ConsiderTypeNameForFindingProperLocatorWhenSaveOperation()
        {
            return true;
        }

        protected override string GetMasterSaveFilePath()
        {
            return Util.GetMasterClassSaveFilePath();
        }

        protected override void OnClear()
        {
            
        }

        protected override T OnLoadWithLocator<T>(ref LocatorForMasterSaveData locator, Action<LocalDataCode, T> OnComplete)
        {
            var fPath = locator.value;
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
            return default;
        }

        protected override void OnSaveWithLocator<T>(ref LocatorForMasterSaveData locator, T data, Action<LocalDataCode> OnComplete)
        {
            var json = Util.GetDataAsJsonString(data);
            var wBytes = CryptoUtil.EncryptIfRequired(json);
            FileWriteUtil.WriteDataAsync(wBytes, locator.value, (success) =>
            {
                OnComplete?.Invoke(success ? LocalDataCode.Success : LocalDataCode.DiskWriteError);
            });
        }

        protected override bool ShouldCreateLocatorIfNotFoundWhenLoadOperation()
        {
            return false;
        }

        protected override bool ShouldLocatorPointFileInDevice()
        {
            return true;
        }
    }
}