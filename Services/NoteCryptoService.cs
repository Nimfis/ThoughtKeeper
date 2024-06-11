﻿using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;

namespace ThoughtKeeper.Service
{
    public class NoteCryptoService : INoteCryptoService
    {
        private readonly byte[] _key = Convert.FromBase64String(ConfigurationManager.AppSettings["notesCryptoAES_Key"]);
        private readonly byte[] _iv = Convert.FromBase64String(ConfigurationManager.AppSettings["notesCryptoAES_IV"]);

        public string EncryptString(string text)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(text);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string DecryptString(string cipherText)
        {
            var buffer = Convert.FromBase64String(cipherText);

            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(buffer))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
