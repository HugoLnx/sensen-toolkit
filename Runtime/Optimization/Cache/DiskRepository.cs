using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;

namespace SensenToolkit
{
    public class DiskRepository
    {
        private readonly string _dirPath;
        private readonly SimpleCryptoBase _simpleCrypto;
        private readonly SimpleHashing _simpleHashing;

        public DiskRepository(string dirPath, SimpleCryptoBase simpleCrypto)
        {
            _dirPath = dirPath;
            _simpleCrypto = simpleCrypto;
            _simpleHashing = SimpleHashing.Instance;
        }

        public static DiskRepository CreateWithDefaultPath(string dirName, SimpleCryptoBase simpleCrypto, bool createIfNotExists = true)
        {
            string rootPath = Application.isEditor
                ? Application.dataPath
                : Application.persistentDataPath;
            rootPath = Path.Combine(rootPath, "_TMP_DISK");
            string dirPath = Path.Combine(rootPath, dirName);
            DiskRepository diskRepo = new(dirPath, simpleCrypto);
            if (createIfNotExists) diskRepo.CreateDirectoryIfNotExists();
            return diskRepo;
        }

        public async Task<(bool, T)> TryGetValue<T>(string key)
        {
            string filePath = Path.Combine(_dirPath, CodeFor(key));
            if (!File.Exists(filePath))
            {
                return (false, default);
            }

            try
            {
                string json = await _simpleCrypto
                    .DecryptFromFile(filePath)
                    .AwaitInAnyThread();
                T value = JSONx.FromJson<T>(json);
                return (true, value);
            } catch (Exception e)
            {
                Debug.LogWarning($"Deleting Disk File. Failed to read it. key={key} path={filePath} error={e}");
                DeleteKey(key);
                return (false, default);
            }
        }

        public async Task SetValue<T>(string key, T value)
        {
            string filePath = Path.Combine(_dirPath, CodeFor(key));

            string json = JSONx.ToJson(value);
            await _simpleCrypto
                .EncryptToFile(json, filePath)
                .AwaitInAnyThread();
        }

        public void Remove(string key)
        {
            string filePath = Path.Combine(_dirPath, CodeFor(key));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void Clear()
        {
            if (Directory.Exists(_dirPath))
            {
                Directory.Delete(_dirPath, true);
            }
            CreateDirectoryIfNotExists();
        }

        public bool Contains(string key)
        {
            string filePath = Path.Combine(_dirPath, CodeFor(key));
            return File.Exists(filePath);
        }

        public IEnumerable<string> GetAllKeys()
        {
            IEnumerable<string> files = Directory.GetFiles(_dirPath);
            return files;
        }

        public void CreateDirectoryIfNotExists()
        {
            if (!Directory.Exists(_dirPath))
            {
                Directory.CreateDirectory(_dirPath);
            }
        }

        private void DeleteKey(string key)
        {
            string filePath = Path.Combine(_dirPath, CodeFor(key));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string CodeFor(string key)
        {
            return _simpleHashing.MD5(key);
        }
    }
}
