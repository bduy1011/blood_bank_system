using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BB.CR.Providers
{
    public static class Algoris
    {
        public static string Encrypt(string key, string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromKey(key);
            aes.IV = DeriveKeyFromKey(key).Take(16).ToArray(); // IV should be 16 bytes for AES

            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(Encoding.UTF8.GetBytes(plainText));
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(output.ToArray());
        }

        public static string Decrypt(string key, string hashText)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromKey(key);
            aes.IV = DeriveKeyFromKey(key).Take(16).ToArray(); // IV should be 16 bytes for AES

            using MemoryStream input = new(Convert.FromBase64String(hashText));
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            cryptoStream.CopyTo(output);

            return Encoding.UTF8.GetString(output.ToArray());
        }

        internal static byte[] DeriveKeyFromKey(string key)
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 32; // 32 bytes equal 256 bits for AES-256
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(key),
                                             emptySalt,
                                             iterations,
                                             hashMethod,
                                             desiredKeyLength);
        }

        private static readonly byte[] IV =
        [
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
        ];
    }

    public class AlgorisMd5
    {
        public static string GetMd5Hash(string input)
        {
            // Convert the input string to a byte array and compute the hash.
            var data = MD5.HashData(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
