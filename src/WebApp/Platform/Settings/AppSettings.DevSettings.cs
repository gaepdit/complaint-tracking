using JetBrains.Annotations;

namespace Cts.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static DevSettingsSection DevSettings { get; private set; } = new();

    // PROD configuration settings
    private static readonly DevSettingsSection ProductionDefault = new()
    {
        UseDevSettings = false,
        UseInMemoryData = false,
        UseEfMigrations = true,
        UseAzureAd = true,
        LocalUserIsAuthenticated = false,
        LocalUserRoles = [],
        UseSecurityHeadersInDev = false,
        EnableWebOptimizer = true,
    };

    // DEV configuration settings
    public record DevSettingsSection
    {
        /// <summary>
        /// Enable (`true`) or disable (`false`) the development settings.
        /// </summary>
        public bool UseDevSettings { get;  init; }

        /// <summary>
        /// Use in-memory data store (`true`) or connect to a SQL Server database (`false`).
        /// </summary>
        public bool UseInMemoryData { get;  init; }

        /// <summary>
        /// Run all Entity Framework migrations (`true`) or create the database based solely on the `DbContext` (`false`).
        /// (Only applies if <see cref="UseInMemoryData"/> is `false`.)
        /// </summary>
        public bool UseEfMigrations { get;  init; }

        /// <summary>
        /// Use Azure AD authentication (`true`) or simulate authentication (`false`).
        /// In order to use Azure AD for authentication, the app must be registered in the Azure portal and
        /// configuration settings added in the `AzureAd` settings section. If `false`, authentication is simulated
        /// using test user data.
        /// </summary>
        public bool UseAzureAd { get;  init; }

        /// <summary>
        /// Simulate a successful login with a test account (`true`) or simulate a failed login (`false`).
        /// (Only applies if <see cref="UseAzureAd"/> is `false`.)
        /// </summary>
        public bool LocalUserIsAuthenticated { get;  init; }

        /// <summary>
        /// Add listed Roles to the logged in test user account.
        /// (Only applies if <see cref="UseAzureAd"/> is `false` and <see cref="LocalUserIsAuthenticated"/> is `true`.)
        /// </summary>
        public string[] LocalUserRoles { get;  init; } = [];

        /// <summary>
        /// Include HTTP security headers when running in a Development environment (`true`).
        /// </summary>
        public bool UseSecurityHeadersInDev { get;  init; }

        /// <summary>
        /// Use WebOptimizer to bundle and minify CSS and JS files (`true`).
        /// </summary>
        public bool EnableWebOptimizer { get;  init; }
    }
}
