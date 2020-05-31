using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace WebAPITest.Utils
{
    public class TokenGenerator
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        public string Encrypt(string key, string plainText)
        {
            RijndaelManaged aes = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                Key = encoding.GetBytes(key)
            };
            aes.GenerateIV();

            ICryptoTransform AESEncrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] buffer = encoding.GetBytes(plainText);

            string encryptedText = Convert.ToBase64String(AESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));

            string mac = "";

            mac = BitConverter.ToString(HmacSHA256(Convert.ToBase64String(aes.IV) + encryptedText, key)).Replace("-", "").ToLower();

            var keyValues = new Dictionary<string, object>
                {
                    { "iv", Convert.ToBase64String(aes.IV) },
                    { "value", encryptedText },
                    { "mac", mac },
                };

            return Convert.ToBase64String(encoding.GetBytes(JsonConvert.SerializeObject(keyValues)));
        }

        public string Decrypt(string key, string plainText)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            aes.Key = encoding.GetBytes(key);

            // Base 64 decode
            byte[] base64Decoded = Convert.FromBase64String(plainText);
            string base64DecodedStr = encoding.GetString(base64Decoded);

            // JSON Decode base64Str
            var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(base64DecodedStr);

            aes.IV = Convert.FromBase64String(payload["iv"]);

            ICryptoTransform AESDecrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] buffer = Convert.FromBase64String(payload["value"]);

            return encoding.GetString(AESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        static byte[] HmacSHA256(string data, string key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(encoding.GetBytes(key)))
            {
                return hmac.ComputeHash(encoding.GetBytes(data));
            }
        }
    }
}
