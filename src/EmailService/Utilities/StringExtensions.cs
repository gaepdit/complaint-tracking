using System.Diagnostics.CodeAnalysis;

namespace GaEpd.EmailService.Utilities;

internal static class StringExtensions
{
    public static string ConcatWithSeparator(this IEnumerable<string?> items, string separator = " ") =>
        string.Join(separator, items.Where(s => !string.IsNullOrEmpty(s)));

    [return: NotNullIfNotNull(nameof(value))]
    public static string? Truncate(this string? value, int maxLength, string suffix = "…")
    {
        if (maxLength < 0) throw new ArgumentException("maxLength must not be negative.", nameof(maxLength));
        if (value is null) return null;
        if (string.IsNullOrEmpty(value) || maxLength == 0) return string.Empty;
        if (value.Length <= maxLength) return value;
        return maxLength < suffix.Length
            ? value[..maxLength]
            : $"{value[..(maxLength - suffix.Length)]}{suffix}";
    }
}
