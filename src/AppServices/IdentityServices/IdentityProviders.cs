using Microsoft.Extensions.Configuration;

namespace Cts.AppServices.IdentityServices;

public static class IdentityProviderValidation
{
    public static bool ValidateIdentityProviderId(this IConfiguration configuration, string loginProvider,
        string identityProviderId)
    {
        if (string.IsNullOrEmpty(identityProviderId)) return false;
        var allowedProviders = configuration.GetSection("AllowedIdentityProviders").Get<IdentityProvider[]>();
        return allowedProviders is not null &&
               allowedProviders.Contains(new IdentityProvider(loginProvider, identityProviderId));
    }

    public static bool ValidateLoginProvider(this IConfiguration configuration, string loginProvider)
    {
        if (string.IsNullOrEmpty(loginProvider)) return false;
        var allowedProviders = configuration.GetSection("AllowedIdentityProviders").Get<IdentityProvider[]>();
        return allowedProviders is not null && allowedProviders.Select(ip => ip.Name).Contains(loginProvider);
    }
}

public record IdentityProvider(string Name, string Id);

public static class IdentityProviders
{
    public const string OktaScheme = "Okta";
    public const string EntraIdScheme = "EntraId";
    public const string TestUserScheme = "TestUser";
}
