using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ComplaintTracking
{
    public static class StringFunctions
    {
        /// <summary>
        /// Implodes a String array to a single string, concatenating the items using the separator, and ignoring null or empty string items
        /// </summary>
        /// <param name="separator">The separator string to include between each item</param>
        /// <param name="items">An array of strings to concatenate</param>
        /// <returns>A concatenated string separated by the specified separator. Null or empty strings are ignored.</returns>
        /// <remarks></remarks>
        public static string ConcatNonEmptyStrings(string[] items, string separator)
        {
            return String.Join(separator, items.Where(s => !String.IsNullOrEmpty(s)));
        }

        public static string RedactPII(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            input = Regex.Replace(input, RegexPatterns.SimpleEmailPattern, RegexPatterns.EmailReplacementText);
            input = Regex.Replace(input, RegexPatterns.SimplePhonePattern, RegexPatterns.PhoneReplacementText);

            return input;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static string ForceToString(object input)
        {
            if (input != null && !string.IsNullOrEmpty(input.ToString()))
            {
                return input.ToString();
            }

            return "";
        }
    }
}
