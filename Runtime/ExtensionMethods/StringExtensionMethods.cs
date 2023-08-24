using UnityEngine;
using System.Text.RegularExpressions;

namespace SensenToolkit
{
    public static class StringExtensionMethods
    {
        private static readonly Regex RegexNonSlugChars = new(@"[^\d\w-]", RegexOptions.Compiled);
        private static readonly Regex RegexBlankChars = new(@"\s+", RegexOptions.Compiled);
        public static string Capitalize(this string str)
        {
            if (str == null || str.Length == 0) return str;
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        public static string ToSlug(this string str)
        {
            str = str.Trim().ToLowerInvariant();
            str = RegexBlankChars.Replace(str, "-");
            str = RegexNonSlugChars.Replace(str, "_");
            return str;
        }
    }
}
