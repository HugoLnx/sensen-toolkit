using System.Collections.Generic;

namespace SensenToolkit
{
    /*
        * This class is a Most Recently Used Cache.
        * It automatically deletes the least recently used item when the cache is full.
        * It uses a pool of nodes to avoid garbage generation internally, but if TContent is a reference type,
        *   it will still generate garbage, unless you keep the reference or/and do something on OnRemove event.
    */
    public class MostRecentlyUsedCache<TKey, TContent> : RecentlyUsedDictionary<TKey, TContent>
    {
        private readonly int _maxSize;

        public MostRecentlyUsedCache(int maxSize)
        : base(nodePoolMaxSize: maxSize)
        {
            _maxSize = maxSize;
        }

        public override void AddOrReplace(TKey key, TContent content)
        {
            if (!Contains(key) && Count >= _maxSize)
            {
                RemoveLeastRecentlyUsed();
            }
            base.AddOrReplace(key, content);
        }
    }
}
