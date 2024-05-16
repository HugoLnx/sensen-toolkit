using System;
using SensenToolkit.Pools;

namespace SensenToolkit
{
    /*
        * This class is a MostRecentlyUsedCache that uses a pool of content to avoid garbage generation
        * when removing content from the cache.
    */
    public class MostRecentlyUsedPooledCache<TKey, TContent>
    {
        private readonly Func<TContent> _factory;
        private readonly SimpleExpandablePool<TContent> _pool;
        private readonly MostRecentlyUsedCache<TKey, TContent> _cache;

        public MostRecentlyUsedPooledCache(
            Func<TContent> factory,
            int minSize = 20,
            int? maxCreations = null,
            bool prefill = true
        )
        {
            _factory = factory;
            _pool = new SimpleExpandablePool<TContent>(factory, minSize, maxCreations, prefill);
            _cache = new MostRecentlyUsedCache<TKey, TContent>(
                maxSize: _pool.MaxCreations
            );
            _cache.OnRemove += (_, value) => _pool.Release(value);
        }

        public TContent Get(TKey key, out bool isNew)
        {
            if (_cache.TryGetValue(key, out TContent content))
            {
                isNew = false;
                return content;
            }
            isNew = true;

            if (!_pool.CanProvide)
            {
                _cache.RemoveLeastRecentlyUsed();
            }

            content = _pool.Get();
            _cache.AddOrReplace(key, content);

            return content;
        }

        public TContent Get(TKey key)
        {
            return Get(key, out _);
        }

        public void Release(TKey key)
        {
            if (_cache.TryGetValue(key, out TContent value))
            {
                _cache.Remove(key);
            }
        }
    }
}
