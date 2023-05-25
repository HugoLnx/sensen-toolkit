using System;
using System.Collections.Generic;

namespace SensenToolkit.Pools
{
    public class SimpleExpandablePool<T> : ISimplePool<T>
    {
        private Func<T> _factory;
        private Queue<T> _resources = new();
        private int _minSize;

        public SimpleExpandablePool(Func<T> factory, int minSize = 20)
        {
            _factory = factory;
            _minSize = minSize;

            for (int i = 0; i < _minSize; i++)
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
            _resources.Enqueue(_factory());
        }
    }
}
