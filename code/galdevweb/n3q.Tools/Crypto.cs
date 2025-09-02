using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace n3q.Tools
{
    public static class Crypto
    {
        public static string SHA1Hex(string input)
        {
            using var sha = SHA1.Create();
            var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hashBytes.Select(b => b.ToString("x2", CultureInfo.InvariantCulture)));
        }

        public static string SHA256Base64(string input)
        {
            using var sha = SHA1.Create();
            var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hashBytes);
        }

        public static byte[] AesEncrypt(string plainText, string key16Bytesas32Hex)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create()) {
                aes.Key = Encoding.UTF8.GetBytes(key16Bytesas32Hex);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using var memoryStream = new MemoryStream();
                using CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream)) {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }

            return array;
        }

        public static byte[] AesEncrypt(byte[] plainData, string key16Bytesas32Hex)
        {
            byte[] iv = new byte[16];
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key16Bytesas32Hex);
            aes.IV = iv;
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainData, 0, plainData.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }

        public static string AesDecryptToString(byte[] cipherBytes, string key16Bytesas32Hex)
        {
            return Encoding.UTF8.GetString(AesDecrypt(cipherBytes, key16Bytesas32Hex));
        }

        public static byte[] AesDecrypt(byte[] cipherBytes, string key16Bytesas32Hex)
        {
            byte[] iv = new byte[16];
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key16Bytesas32Hex);
            aes.IV = iv;
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var msi = new MemoryStream(cipherBytes);
            using var mso = new MemoryStream();
            using (var cs = new CryptoStream(msi, decryptor, CryptoStreamMode.Read)) {
                CopyTo(cs, mso);
            }

            return mso.ToArray();
        }

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
                dest.Write(bytes, 0, cnt);
            }
        }

    }
}
