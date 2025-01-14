using JetBrains.Annotations;

namespace Cts.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    // DEV configuration settings
    public static DevSettingsSection DevSettings { get; set; } = new();

    // PROD configuration settings
    public static readonly DevSettingsSection ProductionDefault = new()
    {
        UseDevSettings = false,
        UseInMemoryData = false,
        UseEfMigrations = true,
        DeleteAndRebuildDatabase = false,
        UseAzureAd = true,
        LocalUserIsAuthenticated = false,
        LocalUserRoles = [],
        UseSecurityHeadersInDev = false,
        EnableWebOptimizer = true,
    };

    public record DevSettingsSection
    {
        /// <summary>
        /// Enable (`true`) or disable (`false`) the development settings.
        /// </summary>
        public bool UseDevSettings { get; [UsedImplicitly] init; }

        /// <summary>
        /// Use in-memory data store (`true`) or connect to a SQL Server database (`false`).
        /// </summary>
        public bool UseInMemoryData { get; [UsedImplicitly] init; }

        /// <summary>
        /// Run Entity Framework migrations (`true`).
        /// If set to `false`, the database will be deleted and recreated on each run or left unmodified,
        /// depending on the value of <see cref="DeleteAndRebuildDatabase"/>.
        /// (Only applies if <see cref="UseInMemoryData"/> is `false`.)
        /// </summary>
        public bool UseEfMigrations { get; [UsedImplicitly] init; }

        /// <summary>
        /// Delete and recreate the database on each run (`true`) or leave the database unmodified (`false`).
        /// If the database does not exist yet, it will not be created if set to `false`.
        /// (Only applies if <see cref="UseInMemoryData"/> and <see cref="UseEfMigrations"/> are both `false`.)
        /// </summary>
        public bool DeleteAndRebuildDatabase { get; [UsedImplicitly] init; }

        /// <summary>
        /// Use Azure AD authentication (`true`) or simulate authentication (`false`).
        /// In order to use Azure AD for authentication, the app must be registered in the Azure portal and
        /// configuration settings added in the `AzureAd` settings section. If `false`, authentication is simulated
        /// using test user data.
        /// </summary>
        public bool UseAzureAd { get; [UsedImplicitly] init; }

        /// <summary>
        /// Simulate a successful login with a test account (`true`) or simulate a failed login (`false`).
        /// (Only applies if <see cref="UseAzureAd"/> is `false`.)
        /// </summary>
        public bool LocalUserIsAuthenticated { get; [UsedImplicitly] init; }

        /// <summary>
        /// Add listed Roles to the logged in test user account.
        /// (Only applies if <see cref="UseAzureAd"/> is `false` and <see cref="LocalUserIsAuthenticated"/> is `true`.)
        /// </summary>
        public string[] LocalUserRoles { get; [UsedImplicitly] init; } = [];

        /// <summary>
        /// Include HTTP security headers when running in a Development environment (`true`).
        /// </summary>
        public bool UseSecurityHeadersInDev { get; [UsedImplicitly] init; }

        /// <summary>
        /// Use WebOptimizer to bundle and minify CSS and JS files (`true`).
        /// </summary>
        public bool EnableWebOptimizer { get; [UsedImplicitly] init; }
    }
}
