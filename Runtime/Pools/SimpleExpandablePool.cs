using System;
using System.Collections.Generic;

namespace SensenToolkit.Pools
{
    public class SimpleExpandablePool<T> : ISimplePool<T>
    {
        private readonly Func<T> _factory;
        private readonly Queue<T> _resources = new();
        private readonly int _minSize;
        private readonly int _maxCreations;
        public HashSet<T> Creations { get; } = new();

        public SimpleExpandablePool(Func<T> factory, int minSize = 20, int? maxCreations = null, bool prefill = true)
        {
            _factory = factory;
            _minSize = minSize;
            _maxCreations = maxCreations ?? minSize + 30;

            if (prefill) Prefill();
        }

        public void Prefill()
        {
            while (Creations.Count < _minSize)
            {
                Grow();
            }
        }

        public T Get()
        {
            if (_resources.Count == 0)
            {
                Grow();
            }
            return _resources.Dequeue();
        }

        public void Release(T obj)
        {
            _resources.Enqueue(obj);
        }

        private void Grow()
        {
            if (Creations.Count >= _maxCreations)
            {
                throw new InvalidOperationException("Pool has reached max it should create");
            }
            T creation = _factory();
            Creations.Add(creation);
            _resources.Enqueue(creation);
        }
    }
}
