using System;
using System.Security.Cryptography;
using System.Text;

namespace SensenToolkit
{
    public static class CryptoUtilities
    {
        public static byte[] GenerateIVFromString(string input)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            byte[] iv = new byte[16];
            Array.Copy(hash, iv, 16);
            return iv;
        }

        public static byte[] DeriveKeyFromString(string input, byte[] salt, int iterations = 100000)
        {
            if (salt == null || salt.Length != 16)
            {
                throw new ArgumentException("Salt should be 16 bytes long.", nameof(salt));
            }

            using Rfc2898DeriveBytes pbkdf2 = new(input, salt, iterations);
            return pbkdf2.GetBytes(32);
        }
    }
}
