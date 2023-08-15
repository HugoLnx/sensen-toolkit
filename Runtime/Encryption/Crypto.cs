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

            using ICryptoTransform encryptor = _aes.CreateEncryptor(encryptionKey, _iv);
            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
            using StreamWriter streamWriter = new(cryptoStream);

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

            using ICryptoTransform decryptor = _aes.CreateDecryptor(encryptionKey, _iv);
            using MemoryStream memoryStream = new(encryptedContent);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);

            return streamReader.ReadToEndAsync();
        }

        public async Task EncryptToFile(string content, string filePath, byte[] encryptionKey)
        {
            if (encryptionKey.Length != 32)
            {
                throw new ArgumentException("Encryption key should be 32 bytes long.", nameof(encryptionKey));
            }

            using ICryptoTransform encryptor = _aes.CreateEncryptor(encryptionKey, _iv);
            using FileStream fileStream = new(filePath, FileMode.Create);
            using CryptoStream cryptoStream = new(fileStream, encryptor, CryptoStreamMode.Write);
            using StreamWriter streamWriter = new(cryptoStream);

            await streamWriter
                .WriteAsync(content)
                .AwaitInAnyThread();
        }

        public async Task<string> DecryptFromFile(string filePath, byte[] encryptionKey)
        {
            if (encryptionKey.Length != 32)
            {
                throw new ArgumentException("Encryption key should be 32 bytes long.", nameof(encryptionKey));
            }

            using ICryptoTransform decryptor = _aes.CreateDecryptor(encryptionKey, _iv);
            using FileStream fileStream = new(filePath, FileMode.Open);
            using CryptoStream cryptoStream = new(fileStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);

            return await streamReader
                .ReadToEndAsync()
                .AwaitInAnyThread();
        }
    }
}
