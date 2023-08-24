using System.Collections.Generic;
using System.Linq;

namespace SensenToolkit
{
    public static class CollectionsExtensionMethods
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => UnityEngine.Random.value);
        }

        public static IEnumerable<IEnumerable<T>> SlicesOf<T>(this IEnumerable<T> source, int size)
        {
            List<T> list = new(size);
            foreach (T item in source)
            {
                list.Add(item);
                if (list.Count == size)
                {
                    yield return list;
                    list = new(size);
                }
            }
            if (list.Count > 0)
            {
                yield return list;
            }
        }

        public static IEnumerable<IEnumerable<T>> SliceByAmount<T>(this IEnumerable<T> source, int amount)
        {
            int size = source.Count() / amount;
            return source.SlicesOf(size);
        }
    }
}
