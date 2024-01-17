using System.Text.RegularExpressions;

namespace ComplaintTracking
{
    public static partial class StringFunctions
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
            return string.Join(separator, items.Where(s => !string.IsNullOrEmpty(s)));
        }

        public static string RedactPII(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            input = SimpleEmailRegex().Replace(input, RegexPatterns.EmailReplacementText);
            input = SimplePhoneRegex().Replace(input, RegexPatterns.PhoneReplacementText);

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

        [GeneratedRegex(RegexPatterns.SimpleEmailPattern)]
        private static partial Regex SimpleEmailRegex();

        [GeneratedRegex(RegexPatterns.SimplePhonePattern)]
        private static partial Regex SimplePhoneRegex();
    }
}
