using System.Collections.Generic;

public static class IEnumeratorExtensionMethods
{
    public static IEnumerable<T> AsEnumerable<T>(this IEnumerator<T> enumerator)
    {
        while (enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }
}
