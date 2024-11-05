using System.Text.RegularExpressions;

namespace Cts.WebApp.Platform.AccountValidation;

public static partial class AccountValidation
{
    public static bool IsValidEmailDomain(this string email) =>
        email.EndsWith("@dnr.ga.gov", StringComparison.CurrentCultureIgnoreCase) ||
        email.EndsWith("@gema.ga.gov", StringComparison.CurrentCultureIgnoreCase);

    public static bool IsTenantAllowed(this IConfiguration configuration, string? tenantId)
    {
        if (string.IsNullOrEmpty(tenantId)) return false;
        var allowedUserTenantIds = configuration.GetSection("AllowedUserTenantIds").Get<string[]>();
        return allowedUserTenantIds is not null && allowedUserTenantIds.Contains(tenantId);
    }

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
