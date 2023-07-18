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
    }
}
