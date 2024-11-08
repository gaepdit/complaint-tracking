using System.Text.RegularExpressions;

namespace Cts.WebApp.Platform.Logging;

public static partial class Redaction
{
    public static string MaskEmail(this string? email)
    {
        if (string.IsNullOrEmpty(email)) return string.Empty;

        var atIndex = email.IndexOf('@');
        if (atIndex <= 1) return email;

        var maskedEmail = MyRegex().Replace(email[..atIndex], "*");
        return string.Concat(maskedEmail, email.AsSpan(atIndex));
    }

    [GeneratedRegex(".(?=.{2})")]
    private static partial Regex MyRegex();
}
