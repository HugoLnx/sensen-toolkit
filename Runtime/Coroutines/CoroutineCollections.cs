using System.Collections;
using System.Collections.Generic;

namespace SensenToolkit
{
    public static class CoroutineCollections
    {
        public static IEnumerator WaitAllSequentially(params IEnumerator[] coroutines)
        {
            foreach (IEnumerator coroutine in coroutines)
            {
                yield return coroutine;
            }
        }
    }
}
