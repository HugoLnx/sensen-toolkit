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
    }
}
