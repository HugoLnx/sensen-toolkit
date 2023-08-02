using System;
using System.Threading.Tasks;

namespace SensenToolkit
{
    public static class TaskSimulator
    {
        public static Task<T> Error<T>(int delaySeconds = 1000)
        {
            return Task
                .Delay(delaySeconds)
                .ContinueWith<T>(
                    _ => throw new Exception("Simulated Error"),
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
        }
        public static Task Error(int delaySeconds = 1000)
        {
            return Task
                .Delay(delaySeconds)
                .ContinueWith(
                    _ => throw new Exception("Simulated Error"),
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
        }
    }
}
