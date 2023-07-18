using UnityEngine;

namespace SensenToolkit
{
    public static class StringExtensionMethods
    {
        public static string Capitalize(this string str)
        {
            if (str == null || str.Length == 0) return str;
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
