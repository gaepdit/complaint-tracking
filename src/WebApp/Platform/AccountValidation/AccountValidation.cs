namespace Cts.WebApp.Platform.AccountValidation;

public static class AccountValidation
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
}
