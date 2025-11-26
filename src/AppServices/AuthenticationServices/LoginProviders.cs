using Microsoft.Extensions.Configuration;

namespace Cts.AppServices.AuthenticationServices;

public static class LoginProviderValidation
{
    private static IEnumerable<LoginProvider>? _loginProviders;
    private static IEnumerable<string>? _loginProviderNames;
    private const string EnabledLoginProviders = nameof(EnabledLoginProviders);

    extension(IConfiguration configuration)
    {
        private IEnumerable<LoginProvider> LoginProviders()
        {
            _loginProviders ??= configuration.GetSection(EnabledLoginProviders).Get<LoginProvider[]>() ?? [];
            return _loginProviders;
        }

        public IEnumerable<string> LoginProviderNames()
        {
            _loginProviderNames ??= configuration.LoginProviders().Select(lp => lp.Name);
            return _loginProviderNames;
        }

        public bool ValidateLoginProviderId(string loginProvider, string identityProviderId)
        {
            if (string.IsNullOrEmpty(loginProvider) || string.IsNullOrEmpty(identityProviderId)) return false;
            return configuration.LoginProviders().Contains(new LoginProvider(loginProvider, identityProviderId));
        }

        public bool ValidateLoginProvider(string loginProvider) =>
            !string.IsNullOrEmpty(loginProvider) && configuration.LoginProviderNames().Contains(loginProvider);
    }
}

public record LoginProvider(string Name, string Id);

public static class LoginProviders
{
    public const string OktaScheme = "Okta";
    public const string EntraIdScheme = "EntraId";
    public const string TestUserScheme = "TestUser";
}
