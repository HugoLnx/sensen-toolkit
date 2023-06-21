using System;

namespace SensenToolkit
{
    public static class Assertx
    {
        public static void IsNotNull(object obj, string message)
        {
            if (obj == null)
            {
                throw new NullReferenceException(message);
            }
        }
    }
}
