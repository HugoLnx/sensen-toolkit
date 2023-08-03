using System;
using System.Threading.Tasks;

namespace SensenToolkit
{
    public static class TaskUtils
    {
        public static Task<T> SimulateError<T>(int delaySeconds = 1000)
        {
            return Task
                .Delay(delaySeconds)
                .ContinueWith<T>(
                    _ => throw new Exception("Simulated Error"),
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
        }
        public static async Task SimulateError(int delaySeconds = 1000)
        {
            await TaskUtils
                .SimulateError<bool>(delaySeconds)
                .AwaitInCurrentThread();
        }

        public static async Task DelayUntil(Func<bool> predicate, int delayMillis = 100)
        {
            while (!predicate())
            {
                await Task.Delay(delayMillis).AwaitInAnyThread();
            }
        }

        public static async Task DelayWhile(Func<bool> predicate, int delayMillis = 100)
        {
            while (predicate())
            {
                await Task.Delay(delayMillis).AwaitInAnyThread();
            }
        }

        public static async Task<T> WithTimeout<T>(Task<T> task, int timeout = 1000)
        {
            if (task == null) return default;
            if (task.IsCompleted) return task.Result;

            Task delayTask = Task.Delay(timeout);
            Task completedTask = await Task.WhenAny(task, delayTask).AwaitInCurrentThread();
            if (completedTask == delayTask)
            {
                throw new TimeoutException();
            }
            return task.Result;
        }

        public static async Task WithTimeout(Task task, int timeout = 1000)
        {
            Task<bool> typedTask = ToTypedTask(task);
            await WithTimeout(typedTask, timeout)
                .AwaitInCurrentThread();
        }

        private static async Task<bool> ToTypedTask(Task task)
        {
            await task.AwaitInCurrentThread();
            return true;
        }
    }
}
