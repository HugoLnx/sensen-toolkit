using System.Collections.Generic;

namespace SensenToolkit.DataStructures
{
    public abstract class ARecentlyUsedDictionary<TKey, TContent>
    {
        public abstract int GetCount();
        public int Count => GetCount();
        public bool IsEmpty => Count == 0;

        protected abstract TKey GetMostRecentlyUsedKey();
        protected abstract TKey GetLeastRecentlyUsedKey();

        public abstract void AddOrReplace(TKey key, TContent value);
        public abstract bool TryGetValue(TKey key, out TContent value);
        protected abstract bool BaseRemove(TKey key, out TContent value);
        public abstract bool Contains(TKey key);

        public event System.Action<TKey, TContent> OnRemove;

        public TContent GetValue(TKey key)
        {
            bool hasKey = TryGetValue(key, out TContent value);
            if (!hasKey)
            {
                throw new KeyNotFoundException($"Key {key} not found in dictionary");
            }
            return value;
        }

        public TContent GetMostRecentlyUsed()
        {
            return GetValue(GetMostRecentlyUsedKey());
        }

        public TContent GetLeastRecentlyUsed()
        {
            return GetValue(GetLeastRecentlyUsedKey());
        }

        public bool TryGetMostRecentlyUsed(out TContent value)
        {
            return TryGetValue(GetMostRecentlyUsedKey(), out value);
        }

        public bool TryGetLeastUsed(out TContent value)
        {
            return TryGetValue(GetLeastRecentlyUsedKey(), out value);
        }

        protected bool Remove(TKey key, out TContent value)
        {
            bool hasRemoved = BaseRemove(key, out value);
            if (hasRemoved)
            {
                OnRemove?.Invoke(key, value);
            }
            return hasRemoved;
        }

        public bool Remove(TKey key)
        {
            return BaseRemove(key, out _);
        }

        public bool RemoveLeastRecentlyUsed()
        {
            return Remove(GetLeastRecentlyUsedKey());
        }

        public bool RemoveMostRecentlyUsed()
        {
            return Remove(GetMostRecentlyUsedKey());
        }

        public bool RemoveLeastRecentlyUsed(out TContent value)
        {
            return BaseRemove(GetLeastRecentlyUsedKey(), out value);
        }

        public bool RemoveMostRecentlyUsed(out TContent value)
        {
            return BaseRemove(GetMostRecentlyUsedKey(), out value);
        }

    }
}
