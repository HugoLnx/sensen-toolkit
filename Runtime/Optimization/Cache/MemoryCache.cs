using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace SensenToolkit
{
    public class MemoryCache : ISimpleCache
    {
        private readonly Dictionary<string, object> _cache = new();

        public Task<(bool, T)> TryGetValue<T>(string key)
        {
            if (_cache.TryGetValue(key, out object obj))
            {
                return Task.FromResult((true, (T)obj));
            }
            return Task.FromResult((false, default(T)));
        }

        public Task SetValue<T>(string key, T value)
        {
            _cache[key] = value;
            return Task.CompletedTask;
        }

        public Task Remove(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public Task Clear()
        {
            _cache.Clear();
            return Task.CompletedTask;
        }

        public Task<bool> Contains(string key)
        {
            return Task.FromResult(_cache.ContainsKey(key));
        }

        public Task<IEnumerable<string>> GetAllKeys()
        {
            IEnumerable<string> keys = _cache.Keys;
            return Task.FromResult(keys);
        }
    }
}
