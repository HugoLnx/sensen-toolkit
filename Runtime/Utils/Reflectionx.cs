using UnityEngine;

namespace SensenToolkit
{
    public static class Reflectionx
    {
        public static bool IsAnonymousMethod(System.Delegate del)
        {
            return del.Method.Name.Contains("<");
        }
    }
}
