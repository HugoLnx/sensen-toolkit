using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SensenToolkit
{
    public class Crypto
    {
        private readonly byte[] _iv;
        private readonly Aes _aes;

        public Crypto(byte[] iv)
        {
            if (iv.Length != 16)
            {
                throw new ArgumentException("IV should be 16 bytes long.", nameof(iv));
            }

            _iv = iv;
            _aes = Aes.Create();
            _aes.KeySize = 256;
            _aes.BlockSize = 128;
            _aes.Mode = CipherMode.CBC;
            _aes.Padding = PaddingMode.PKCS7;
        }

        public async Task<byte[]> Encrypt(string content, byte[] encryptionKey)
        {
            if (encryptionKey.Length != 32)
            {
                throw new ArgumentException("Encryption key should be 32 bytes long.", nameof(encryptionKey));
            }

            using var encryptor = _aes.CreateEncryptor(encryptionKey, _iv);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using var streamWriter = new StreamWriter(cryptoStream);

            await streamWriter.WriteAsync(content);
            await streamWriter.FlushAsync();
            cryptoStream.FlushFinalBlock();

            return memoryStream.ToArray();
        }

        public Task<string> Decrypt(byte[] encryptedContent, byte[] encryptionKey)
        {
            if (encryptionKey.Length != 32)
            {
                throw new ArgumentException("Encryption key should be 32 bytes long.", nameof(encryptionKey));
            }

            using var decryptor = _aes.CreateDecryptor(encryptionKey, _iv);
            using var memoryStream = new MemoryStream(encryptedContent);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEndAsync();
        }
    }
}
