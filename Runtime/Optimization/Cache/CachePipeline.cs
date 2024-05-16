using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensenToolkit
{
    public class CachePipeline : ISimpleCache
    {
        private readonly ISimpleCache[] _pipeline;
        private readonly List<ISimpleCache> _buffer = new();
        private ISimpleCache BottomCache => _pipeline[^1];

        public CachePipeline(ISimpleCache[] pipeline)
        {
            _pipeline = pipeline;
        }

        public async Task<(bool, T)> TryGetValue<T>(string key)
        {
            _buffer.Clear();
            List<ISimpleCache> cachesWithoutKey = _buffer;
            foreach (var cache in _pipeline)
            {
                var (hasFound, value) = await cache
                    .TryGetValue<T>(key)
                    .AwaitInAnyThread();
                if (hasFound)
                {
                    await SetOnAll(key, value, cachesWithoutKey);
                    return (true, value);
                } else
                {
                    cachesWithoutKey.Add(cache);
                }
            }
            return (false, default(T));
        }

        public Task SetValue<T>(string key, T value)
        {
            return Task.WhenAll(_pipeline.Select(
                cache => cache.SetValue(key, value)));
        }

        public Task Remove(string key)
        {
            return Task.WhenAll(_pipeline.Select(
                cache => cache.Remove(key)));
        }

        public Task Clear()
        {
            return Task.WhenAll(_pipeline.Select(
                cache => cache.Clear()));
        }

        public async Task<bool> Contains(string key)
        {
            foreach (var cache in _pipeline)
            {
                bool hasKey = await cache
                    .Contains(key)
                    .AwaitInAnyThread();
                if (hasKey)
                {
                    return true;
                }
            }
            return false;
        }

        private static Task SetOnAll<T>(string key, T value, List<ISimpleCache> caches)
        {
            return Task.WhenAll(caches.Select(
                cache => cache.SetValue(key, value)));
        }

        public Task<IEnumerable<string>> GetAllKeys()
        {
            return BottomCache.GetAllKeys();
        }
    }
}
