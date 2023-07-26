using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace SensenToolkit
{
    public static class TaskExtensionMethods
    {
        public static IEnumerator WaitForCompletion(this Task task)
        {
            return new WaitUntil(() => task.IsCompleted);
        }

        public static IEnumerator WaitForCompletion<T>(this Task<T> task)
        {
            return new WaitUntil(() => task.IsCompleted);
        }

        public static async Task<T> RetryOnError<T>(this Task<T> task, int maxRetries = 3, float delay = 0.1f, float delayIncrement = 1f)
        {
            try
            {
                return await task;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                if (maxRetries > 0)
                {
                    await Task.Delay((int)(delay * 1000)).ConfigureAwait(false);
                    return await task.RetryOnError(maxRetries - 1, delay + delayIncrement, delayIncrement + 1).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task RetryOnError(this Task task, int maxRetries = 3, float delay = 0.1f, float delayIncrement = 1f)
        {
            await task
            .ContinueWith(t => true)
            .RetryOnError(maxRetries, delay, delayIncrement)
            .ConfigureAwait(false);
        }
    }
}
