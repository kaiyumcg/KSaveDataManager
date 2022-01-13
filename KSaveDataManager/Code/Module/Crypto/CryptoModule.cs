using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace KSaveDataMan
{
    internal static class CryptoUtil
    {
        #region GlobalCrypt
        internal static byte[] EncryptStringToBytes(string plainText, string key, string iv)
        {
            byte[] Key = Convert.FromBase64String(key);
            byte[] IV = Convert.FromBase64String(iv);
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

        internal static string DecryptStringFromBytes(byte[] data, string key, string iv)
        {
            byte[] Key = Convert.FromBase64String(key);
            byte[] IV = Convert.FromBase64String(iv);
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

        internal static byte[] EncryptIfSettingPermitsOtherwisePaintTxt(string str, string key, string iv)
        {
            byte[] saveBytes = null;
            var useEncryption = false;
            if (Config.data != null && Config.data.EncryptionConfig != null)
            {
                useEncryption = true;
            }

            if (useEncryption)
            {
                saveBytes = CryptoUtil.EncryptStringToBytes(str, key, iv);
            }
            else
            {
                var encoding = EncodingType.ASCII;
                if (Config.data != null) { encoding = Config.data._EncodingType; }
                if (encoding == EncodingType.ASCII)
                {
                    saveBytes = Encoding.ASCII.GetBytes(str);
                }
                else if (encoding == EncodingType.BIG_ENDIAN)
                {
                    saveBytes = Encoding.BigEndianUnicode.GetBytes(str);
                }
                else if (encoding == EncodingType.UNICODE)
                {
                    saveBytes = Encoding.Unicode.GetBytes(str);
                }
                else if (encoding == EncodingType.UTF_32)
                {
                    saveBytes = Encoding.UTF32.GetBytes(str);
                }
                else if (encoding == EncodingType.UTF_7)
                {
                    saveBytes = Encoding.UTF7.GetBytes(str);
                }
                else if (encoding == EncodingType.UTF_8)
                {
                    saveBytes = Encoding.UTF8.GetBytes(str);
                }
            }
            return saveBytes;
        }

        internal static string DecryptIfSettingPermitsOtherwisePaintTxt(byte[] data, string key, string iv)
        {
            string result = "";
            var useEncryption = false;
            if (Config.data != null && Config.data.EncryptionConfig != null)
            {
                useEncryption = true;
            }
            if (useEncryption)
            {
                result = CryptoUtil.DecryptStringFromBytes(data, key, iv);
            }
            else
            {
                var encoding = EncodingType.ASCII;
                if (Config.data != null) { encoding = Config.data._EncodingType; }
                if (encoding == EncodingType.ASCII)
                {
                    result = Encoding.ASCII.GetString(data, 0, data.Length);
                }
                else if (encoding == EncodingType.BIG_ENDIAN)
                {
                    result = Encoding.BigEndianUnicode.GetString(data, 0, data.Length);
                }
                else if (encoding == EncodingType.UNICODE)
                {
                    result = Encoding.Unicode.GetString(data, 0, data.Length);
                }
                else if (encoding == EncodingType.UTF_32)
                {
                    result = Encoding.UTF32.GetString(data, 0, data.Length);
                }
                else if (encoding == EncodingType.UTF_7)
                {
                    result = Encoding.UTF7.GetString(data, 0, data.Length);
                }
                else if (encoding == EncodingType.UTF_8)
                {
                    result = Encoding.UTF8.GetString(data, 0, data.Length);
                }
            }
            return result;
        }
        #endregion

        #region PerDataSpecificCrypt

        static byte[] GetSalt(string saltString, EncodingType encodingType)
        {
            byte[] salt = null;
            if (encodingType == EncodingType.ASCII)
            {
                salt = Encoding.ASCII.GetBytes(saltString);
            }
            else if (encodingType == EncodingType.BIG_ENDIAN)
            {
                salt = Encoding.BigEndianUnicode.GetBytes(saltString);
            }
            else if (encodingType == EncodingType.UNICODE)
            {
                salt = Encoding.Unicode.GetBytes(saltString);
            }
            else if (encodingType == EncodingType.UTF_32)
            {
                salt = Encoding.UTF32.GetBytes(saltString);
            }
            else if (encodingType == EncodingType.UTF_7)
            {
                salt = Encoding.UTF7.GetBytes(saltString);
            }
            else if (encodingType == EncodingType.UTF_8)
            {
                salt = Encoding.UTF8.GetBytes(saltString);
            }
            return salt;
        }

        //Src: https://www.c-sharpcorner.com/UploadFile/a85b23/text-encrypt-and-decrypt-with-a-specified-key/
        /// <summary>  
        /// Encrypt the given string using AES.  The string can be decrypted using  
        /// DecryptStringAES().  The sharedSecret parameters must match.  
        /// </summary>  
        /// <param name="plainText">The text to encrypt.</param>  
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>  
        internal static string EncryptStringAES(string plainText, string sharedSecret, EncodingType encodingType, string salt)
        {
            var _salt = GetSalt(salt, encodingType);
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                 // Encrypted string to return  
            RijndaelManaged aesAlg = null;        // RijndaelManaged object used to encrypt the data.  

            try
            {
                // generate the key from the shared secret and the salt  
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create a RijndaelManaged object  
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.  
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.  
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV  
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt =
                       new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            catch (Exception ex)
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogError("could not encrypt per data save. Exception msg: " + ex.Message);
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.  
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.  
            return outStr;   
        }

        /// <summary>  
        /// Decrypt the given string.  Assumes the string was encrypted using  
        /// EncryptStringAES(), using an identical sharedSecret.  
        /// </summary>  
        /// <param name="cipherText">The text to decrypt.</param>  
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>  
        internal static string DecryptStringAES(string cipherText, string sharedSecret, EncodingType encodingType, string salt)
        {
            var _salt = GetSalt(salt, encodingType);
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object  
            // used to decrypt the data.  
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt  
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create the streams used for decryption.  
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object  
                    // with the specified key and IV.  
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream  
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.  
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt =
                        new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream  
                            // and place them in a string.  
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                if (Config.data != null && Config.data.DebugMessage)
                {
                    Debug.LogError("could not decrypt per data save. Exception msg: " + ex.Message);
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return plaintext;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
        #endregion
    }
}