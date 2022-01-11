using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace KSaveDataMan
{
    internal static class CryptoUtil
    {
        internal static EncryptionUsageDescription encryptionSetting = null;
        internal static byte[] EncryptStringToBytes(string plainText)
        {
            byte[] Key = Convert.FromBase64String(encryptionSetting.KEY);
            byte[] IV = Convert.FromBase64String(encryptionSetting.IV);
            byte[] auditTrail;
            using (Aes newAes = Aes.Create())
            {
                newAes.Key = Key;
                newAes.IV = IV;

                ICryptoTransform cryptDesc = newAes.CreateEncryptor(Key, IV);

                using (MemoryStream cryptMemStream = new MemoryStream())
                {
                    using (CryptoStream crpt_stream = new CryptoStream(cryptMemStream, cryptDesc, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(crpt_stream))
                        {
                            swEncrypt.Write(plainText);
                        }
                        auditTrail = cryptMemStream.ToArray();
                    }
                }
            }

            return auditTrail;
        }

        internal static string DecryptStringFromBytes(byte[] data)
        {
            byte[] Key = Convert.FromBase64String(encryptionSetting.KEY);
            byte[] IV = Convert.FromBase64String(encryptionSetting.IV);
            string decryptText;
            using (Aes newAes = Aes.Create())
            {
                newAes.Key = Key;
                newAes.IV = IV;
                ICryptoTransform decryptDesc = newAes.CreateDecryptor(Key, IV);
                using (MemoryStream memStream = new MemoryStream(data))
                {
                    using (CryptoStream cryptStream = new CryptoStream(memStream, decryptDesc, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDcrpt = new StreamReader(cryptStream))
                        {
                            decryptText = srDcrpt.ReadToEnd();
                        }
                    }
                }
            }
            return decryptText;
        }
    }
}