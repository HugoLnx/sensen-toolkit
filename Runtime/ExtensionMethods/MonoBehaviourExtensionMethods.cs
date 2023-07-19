using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SensenToolkit
{
    public static class MonoBehaviourExtensionMethods
    {
        public static IEnumerator StartCoroutinesInParallel(this MonoBehaviour mono, params IEnumerator[] enumerators)
        {
            List<Coroutine> coroutines = enumerators
                .Select(coroutine => mono.StartCoroutine(coroutine))
                .ToList();

            foreach (Coroutine coroutine in coroutines)
            {
                yield return coroutine;
            }
        }
    }
}
