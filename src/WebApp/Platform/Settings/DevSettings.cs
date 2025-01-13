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
        LocalUserIsStaff = false,
        LocalUserIsAdmin = false,
        UseSecurityHeadersInDev = false,
        EnableWebOptimizer = true,
    };

    public record DevSettingsSection
    {
        /// <summary>
        /// Set to `true` to enable the development settings.
        /// </summary>
        public bool UseDevSettings { get; [UsedImplicitly] init; }

        /// <summary>
        /// Uses in-memory data store when `true`. Connects to a SQL Server database when `false`.
        /// </summary>
        public bool UseInMemoryData { get; [UsedImplicitly] init; }

        /// <summary>
        /// Uses Entity Framework migrations when `true`.
        /// (Only applies if <see cref="UseInMemoryData"/> is `false`.)
        /// </summary>
        public bool UseEfMigrations { get; [UsedImplicitly] init; }

        /// <summary>
        /// When set to `true`, the database is deleted and recreated on each run. When set to `false`, the database
        /// is not modified on each run. (If the database does not exist yet, it will not be created if this is set
        /// to `false`.)
        /// (Only applies if <see cref="UseInMemoryData"/> and <see cref="UseEfMigrations"/> are both `false`.)
        /// </summary>
        public bool DeleteAndRebuildDatabase { get; [UsedImplicitly] init; }

        /// <summary>
        /// If `true`, the app must be registered in the Azure portal, and configuration settings added in the
        /// "AzureAd" settings section. If `false`, authentication is simulated using test user data.
        /// </summary>
        public bool UseAzureAd { get; [UsedImplicitly] init; }

        /// <summary>
        /// Simulates a successful login with a test account when `true`. Simulates a failed login when `false`.
        /// (Only applies if <see cref="UseAzureAd"/> is `false`.)
        /// </summary>
        public bool LocalUserIsAuthenticated { get; [UsedImplicitly] init; }

        /// <summary>
        /// Adds the Staff and Site Maintenance roles when `true` or no roles when `false`.
        /// (Only applies if <see cref="LocalUserIsAuthenticated"/> is `true`.)
        /// </summary>
        public bool LocalUserIsStaff { get; [UsedImplicitly] init; }

        /// <summary>
        /// Adds all App Roles to the logged in account when `true` or no roles when `false`. (Applies whether
        /// <see cref="UseAzureAd"/> is `true` or `false`.)
        /// </summary>
        public bool LocalUserIsAdmin { get; [UsedImplicitly] init; }

        /// <summary>
        /// Sets whether to include HTTP security headers when running locally in the Development environment.
        /// </summary>
        public bool UseSecurityHeadersInDev { get; [UsedImplicitly] init; }

        /// <summary>
        /// Sets whether to use WebOptimizer to bundle and minify CSS and JS files.
        /// </summary>
        public bool EnableWebOptimizer { get; [UsedImplicitly] init; }
    }
}
