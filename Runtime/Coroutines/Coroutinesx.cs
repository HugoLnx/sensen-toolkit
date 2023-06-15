using System;

namespace SensenToolkit
{
    public static class Coroutinesx
    {
        public static TimedWaitWhile TimedWaitWhile(Func<bool> condition, float timeout)
            => new TimedWaitWhile(condition, timeout);
        public static TimedWaitWhile TimedWaitUntil(Func<bool> condition, float timeout)
            => new TimedWaitWhile(() => !condition(), timeout);
    }
}
