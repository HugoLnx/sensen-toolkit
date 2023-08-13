using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SensenToolkit
{
    public class DiskCache : ISimpleCache
    {
        private readonly string _dirPath;
        private readonly SimpleCryptoBase _simpleCrypto;
        private readonly SimpleHashing _simpleHashing;

        public DiskCache(string dirPath, SimpleCryptoBase simpleCrypto)
        {
            _dirPath = dirPath;
            _simpleCrypto = simpleCrypto;
            _simpleHashing = SimpleHashing.Instance;
        }

        public static DiskCache CreateWithDefaultPath(string dirName, SimpleCryptoBase simpleCrypto, bool createIfNotExists = true)
        {
            string rootPath = Application.isEditor
                ? Application.dataPath
                : Application.persistentDataPath;
            rootPath = Path.Combine(rootPath, "_CACHE_TMP");
            string dirPath = Path.Combine(rootPath, dirName);
            DiskCache cache = new(dirPath, simpleCrypto);
            if (createIfNotExists) cache.CreateDirectoryIfNotExists();
            return cache;
        }

        public async Task<(bool, T)> TryGetValue<T>(string key)
        {
            string filePath = Path.Combine(_dirPath, CodeFor(key));
            if (!File.Exists(filePath))
            {
                return (false, default);
            }

            byte[] encryptedJson = await File
                .ReadAllBytesAsync(filePath)
                .AwaitInAnyThread();
            string json = await _simpleCrypto
                .Decrypt(encryptedJson)
                .AwaitInAnyThread();
            T value = JSONx.FromJson<T>(json);
            return (true, value);
        }

        public async Task SetValue<T>(string key, T value)
        {
            string filePath = Path.Combine(_dirPath, key);

            string json = JSONx.ToJson(value);
            byte[] encryptedJson = await _simpleCrypto
                .Encrypt(json)
                .AwaitInAnyThread();
            File.WriteAllBytes(filePath, encryptedJson);
        }

        public Task Remove(string key)
        {
            string filePath = Path.Combine(_dirPath, CodeFor(key));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        public Task Clear()
        {
            if (Directory.Exists(_dirPath))
            {
                Directory.Delete(_dirPath, true);
            }
            CreateDirectoryIfNotExists();
            return Task.CompletedTask;
        }

        public Task<bool> Contains(string key)
        {
            string filePath = Path.Combine(_dirPath, CodeFor(key));
            return Task.FromResult(File.Exists(filePath));
        }

        public Task<IEnumerable<string>> GetAllKeys()
        {
            IEnumerable<string> files = Directory.GetFiles(_dirPath);
            return Task.FromResult(files);
        }

        public void CreateDirectoryIfNotExists()
        {
            if (!Directory.Exists(_dirPath))
            {
                Directory.CreateDirectory(_dirPath);
            }
        }

        private string CodeFor(string key)
        {
            return _simpleHashing.MD5(key);
        }
    }
}