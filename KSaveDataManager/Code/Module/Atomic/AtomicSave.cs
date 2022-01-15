using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
using System;

namespace KSaveDataMan
{
    internal class AtomicSave : StructuredSave
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
            return Util.GetMasterAtomicSaveFilePath();
        }

        protected override void OnClear()
        {
            
        }

        protected override T OnLoadWithLocator<T>(ref LocatorForMasterSaveData locator, Action<LocalDataCode, T> OnComplete)
        {
            var jsonOrPlainDataString = locator.value;
            if (Config.data != null && Config.data.EncryptionConfig != null && Config.data.EncryptionConfig.AtomicEncryptEachData)
            {
                var sharedSecret = Config.data.EncryptionConfig.AtomicPerDataSharedSecret;
                var salt = Config.data.EncryptionConfig.AtomicPerDataSalt;
                var encoding = Config.data.EncryptionConfig.AtomicPerDataCredentialEncoding;
                jsonOrPlainDataString = CryptoUtil.DecryptStringAES(locator.value, sharedSecret, encoding, salt);
            }
            var data = Util.GetAtomicValueFromStringOrJson<T>(jsonOrPlainDataString);
            OnComplete?.Invoke(LocalDataCode.Success, data);
            return data;
        }

        protected override void OnSaveWithLocator<T>(ref LocatorForMasterSaveData locator, T data, Action<LocalDataCode> OnComplete)
        {
            var jsonData = Util.ConvertAtomicValueToJsonString<T>(data);
            var locatorValue = jsonData;
            if (Config.data != null && Config.data.EncryptionConfig != null && Config.data.EncryptionConfig.AtomicEncryptEachData)
            {
                var sharedSecret = Config.data.EncryptionConfig.AtomicPerDataSharedSecret;
                var salt = Config.data.EncryptionConfig.AtomicPerDataSalt;
                var encoding = Config.data.EncryptionConfig.AtomicPerDataCredentialEncoding;
                locatorValue = CryptoUtil.EncryptStringAES(locatorValue, sharedSecret, encoding, salt);
            }
            locator.value = locatorValue;
            OnComplete?.Invoke(LocalDataCode.Success);
        }

        protected override bool ShouldCreateLocatorIfNotFoundWhenLoadOperation()
        {
            return true;
        }

        protected override bool ShouldLocatorPointFileInDevice()
        {
            return false;
        }
    }
}