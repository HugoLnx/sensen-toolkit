using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit
{
    public static class CoroutineCollections
    {
        public static IEnumerator WaitAllSequentially(params IEnumerator[] coroutines)
        {
            return WaitAllSequentially((IEnumerable<IEnumerator>) coroutines);
        }
        public static IEnumerator WaitAllSequentially(IEnumerable<IEnumerator> coroutines)
        {
            foreach (IEnumerator coroutine in coroutines)
            {
                yield return coroutine;
            }
        }
    }
}
