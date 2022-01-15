using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KSaveDataMan
{
    internal class BigDataSave : StructuredSave
    {
        protected override bool ConsiderTypeNameForFindingProperLocatorWhenLoadOperation()
        {
            return false;
        }

        protected override bool ConsiderTypeNameForFindingProperLocatorWhenSaveOperation()
        {
            return false;
        }

        protected override string GetMasterSaveFilePath()
        {
            return Util.GetMasterSaveFilePathForBigData();
        }

        protected override void OnClear()
        {
            
        }

        protected override T OnLoadWithLocator<T>(ref LocatorForMasterSaveData locator, Action<LocalDataCode, T> OnComplete)
        {
            var fPath = locator.value;
            FileLoadUtil.LoadDataAsync(fPath, (success, rawByteData) =>
            {
                if (success)
                {
                    OnComplete?.Invoke(rawByteData == null || rawByteData.Length == 0 ? LocalDataCode.DiskLoadError : LocalDataCode.Success, (T)(object)rawByteData);
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
            byte[] rawByteData = (byte[])(object)data;
            FileWriteUtil.WriteDataAsync(rawByteData, locator.value, (success) =>
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