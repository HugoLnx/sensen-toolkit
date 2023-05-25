using System.Collections;
using System.Collections.Generic;
using SensenToolkit.Pools;
using UnityEngine;

namespace SensenToolkit.Coroutines
{
    public static class GarbagelessCoroutines
    {
        private static readonly SimpleExpandablePool<WaitForSecondsRealtime> _waitSecondsRealtimePool = new(() => new(0f), minSize: 100);
        private static readonly Dictionary<int, WaitForSeconds> _waitForSecondsDictionary = new();
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new();
        private const float _waitForSecondsMaxPrecision = 1e-3f;
        public static YieldInstruction WaitForSeconds(float seconds)
        {
            int id = Mathf.RoundToInt(seconds / _waitForSecondsMaxPrecision);
            if (!_waitForSecondsDictionary.TryGetValue(id, out var waitForSeconds))
            {
                waitForSeconds = new WaitForSeconds(id *_waitForSecondsMaxPrecision);
                _waitForSecondsDictionary[id] = waitForSeconds;
            }
            return waitForSeconds;
        }

        public static IEnumerator WaitForSecondsRealtime(float seconds)
        {
            WaitForSecondsRealtime waitRealtime = _waitSecondsRealtimePool.Get();
            waitRealtime.waitTime = seconds;
            yield return waitRealtime;
            _waitSecondsRealtimePool.Release(waitRealtime);
        }
    }
}
