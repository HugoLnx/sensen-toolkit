using System;
using System.Collections.Generic;
using SensenToolkit.Pools;

namespace SensenToolkit.DataStructures
{
    /*
        * This class is a dictionary that keeps track of the most recently used keys.
        * It is implemented using a dictionary and a linked list.
        * It can have a pool of nodes to avoid garbage generation up to a certain limit of nodes.
    */
    public class RecentlyUsedDictionary<TKey, TContent> : ARecentlyUsedDictionary<TKey, TContent>
    {
        private class PairKeyNode
        {
            public TContent Value { get; set; }
            public LinkedListNode<TKey> Node { get; set; }
            public TKey Key
            {
                get => Node.Value;
                set => Node.Value = value;
            }
        }
        private readonly LinkedList<TKey> _linkedKeys = new();
        private readonly Dictionary<TKey, PairKeyNode> _dictionary = new();
        private readonly SimpleExpandablePool<PairKeyNode> _pairPool;

        public RecentlyUsedDictionary(int nodePoolMaxSize = 20)
        {
            _pairPool = new SimpleExpandablePool<PairKeyNode>(
                factory: () => CreateBlankPairNode(key: default),
                minSize: 0,
                maxCreations: nodePoolMaxSize,
                prefill: false
            );
        }

        public override int GetCount()
        {
            return _linkedKeys.Count;
        }

        protected override TKey GetMostRecentlyUsedKey()
        {
            return IsEmpty
                ? throw new InvalidOperationException("Cannot get most recently used key from empty dictionary")
                : _linkedKeys.Last.Value;
        }

        protected override TKey GetLeastRecentlyUsedKey()
        {
            return IsEmpty
                ? throw new InvalidOperationException("Cannot get least recently used key from empty dictionary")
                : _linkedKeys.First.Value;
        }

        public override void AddOrReplace(TKey key, TContent value)
        {
            if (!_dictionary.TryGetValue(key, out PairKeyNode pair))
            {
                pair = GetOrCreatePair(key, value);
            }
            pair.Value = value;
            AddOrMoveToMostRecentlyUsed(pair);
        }

        public override bool TryGetValue(TKey key, out TContent value)
        {
            if (_dictionary.TryGetValue(key, out PairKeyNode pair))
            {
                AddOrMoveToMostRecentlyUsed(pair);
                value = pair.Value;
                return true;
            }
            value = default;
            return false;
        }

        protected override bool BaseRemove(TKey key, out TContent value)
        {
            if (!_dictionary.TryGetValue(key, out PairKeyNode pair))
            {
                value = default;
                return false;
            }
            _dictionary.Remove(key);
            _linkedKeys.Remove(pair.Node);
            value = pair.Value;
            pair.Key = default;
            pair.Value = default;
            _pairPool.Release(pair);
            return true;
        }

        public override bool Contains(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        private void AddOrMoveToMostRecentlyUsed(PairKeyNode pair)
        {
            LinkedListNode<TKey> node = pair.Node;
            if (!IsEmpty && pair.Key.Equals(GetMostRecentlyUsedKey())) return;
            if (_dictionary.ContainsKey(pair.Key))
            {
                _linkedKeys.Remove(node);
            }
            else
            {
                _dictionary.Add(pair.Key, pair);
            }
            _linkedKeys.AddLast(pair.Key);
        }

        private PairKeyNode GetOrCreatePair(TKey key, TContent value)
        {
            PairKeyNode pair;
            if (_pairPool.CanProvide)
            {
                pair = _pairPool.Get();
            }
            else
            {
                pair = CreateBlankPairNode(key);
            }
            pair.Key = key;
            pair.Value = value;
            return pair;
        }

        private PairKeyNode CreateBlankPairNode(TKey key)
        {
            return new PairKeyNode
            {
                Node = new LinkedListNode<TKey>(key)
            };
        }
    }
}
