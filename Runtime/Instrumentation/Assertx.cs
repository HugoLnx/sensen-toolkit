using System;
using System.Collections;
using System.Collections.Generic;

namespace SensenToolkit
{
    public static class Assertx
    {
        public static void IsNotEmpty<T>(IReadOnlyCollection<T> collection, string message = null)
        {
            IsNotNull(collection, "collection is null");
            if (collection.Count == 0)
            {
                throw new InvalidOperationException(FormatMessage(message, "collection is empty"));
            }
        }

        public static void IsNotNull(object obj, string message = null)
        {
            if (obj == null)
            {
                throw new NullReferenceException(FormatMessage(message, "object is null"));
            }
        }

        public static void IsTrue(bool v, string message)
        {
            if (!v)
            {
                throw new InvalidOperationException(FormatMessage(message, "condition is false"));
            }
        }

        public static void IsFalse(bool v, string message)
        {
            if (v)
            {
                throw new InvalidOperationException(FormatMessage(message, "condition is true"));
            }
        }

        private static string FormatMessage(string message, string defaultMessage)
        {
            return message ?? $"Assetx failed: {defaultMessage}";
        }
    }
}
