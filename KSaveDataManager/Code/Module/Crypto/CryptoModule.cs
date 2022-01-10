using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public static class CryptoUtil
{
    internal static string _Key_Str = "AsISxq9OwdZag1163OJqwovXfSWG98m+sPjVwJecfe4=";
    internal static string _IV_Str = "Aq0UThtJhjbuyWXtmZs1rw==";

    private static byte[] EncryptStringToBytes(string plainText)
    {
        byte[] Key = Convert.FromBase64String(_Key_Str);
        byte[] IV = Convert.FromBase64String(_IV_Str);

        byte[] encryptedAuditTrail;

        using (Aes newAes = Aes.Create())
        {
            newAes.Key = Key;
            newAes.IV = IV;

            ICryptoTransform encryptor = newAes.CreateEncryptor(Key, IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encryptedAuditTrail = msEncrypt.ToArray();
                }
            }
        }

        return encryptedAuditTrail;
    }

    private static string DecryptStringFromBytes(byte[] data)
    {
        byte[] Key = Convert.FromBase64String(_Key_Str);
        byte[] IV = Convert.FromBase64String(_IV_Str);
        string decryptText;

        using (Aes newAes = Aes.Create())
        {
            newAes.Key = Key;
            newAes.IV = IV;

            ICryptoTransform decryptor = newAes.CreateDecryptor(Key, IV);

            using (MemoryStream msDecrypt = new MemoryStream(data))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        decryptText = srDecrypt.ReadToEnd();
                    }
                }
            }
        }


        return decryptText;
    }
}
