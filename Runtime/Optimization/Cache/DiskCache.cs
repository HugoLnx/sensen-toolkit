using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;

namespace SensenToolkit
{
    public class DiskCache : ISimpleCache
    {
        private readonly DiskRepository _diskRepo;

        public DiskCache(string dirPath, SimpleCryptoBase simpleCrypto)
        : this(new DiskRepository(dirPath, simpleCrypto))
        { }

        public DiskCache(DiskRepository diskRepo)
        {
            _diskRepo = diskRepo;
        }

        public static DiskCache CreateWithDefaultPath(string dirName, SimpleCryptoBase simpleCrypto, bool createIfNotExists = true)
        {
            DiskRepository diskRepo = DiskRepository.CreateWithDefaultPath(dirName, simpleCrypto, createIfNotExists);
            return new DiskCache(diskRepo);
        }

        public async Task<(bool, T)> TryGetValue<T>(string key)
        {
            return await _diskRepo.TryGetValue<T>(key).AwaitInAnyThread();
        }

        public async Task SetValue<T>(string key, T value)
        {
            await _diskRepo.SetValue<T>(key, value).AwaitInAnyThread();
        }

        public Task Remove(string key)
        {
            _diskRepo.Remove(key);
            return Task.CompletedTask;
        }

        public Task Clear()
        {
            _diskRepo.Clear();
            return Task.CompletedTask;
        }

        public Task<bool> Contains(string key)
        {
            return Task.FromResult(_diskRepo.Contains(key));
        }

        public Task<IEnumerable<string>> GetAllKeys()
        {
            return Task.FromResult(_diskRepo.GetAllKeys());
        }

        public void CreateDirectoryIfNotExists()
        {
            _diskRepo.CreateDirectoryIfNotExists();
        }
    }
}
