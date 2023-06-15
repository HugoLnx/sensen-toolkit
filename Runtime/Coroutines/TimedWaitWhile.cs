using System;
using System.Collections;
using UnityEngine;

namespace SensenToolkit
{
    public class TimedWaitWhile
    {
        private const float WaitDelayTime = 0.1f;
        private static readonly WaitForSeconds WaitDelay = new WaitForSeconds(WaitDelayTime);
        private readonly Func<bool> condition;
        private float timeout;
        public bool HasTimedout {get; private set;} = false;

        public TimedWaitWhile(Func<bool> condition, float timeout)
        {
            this.condition = condition;
            this.timeout = timeout;
        }

        public IEnumerator Wait()
        {
            while (condition() && timeout > 0f)
            {
                yield return WaitDelay;
                timeout -= WaitDelayTime;
            }
            if (timeout <= 0)
            {
                this.HasTimedout = true;
                yield break;
            }
        }
    }
}
