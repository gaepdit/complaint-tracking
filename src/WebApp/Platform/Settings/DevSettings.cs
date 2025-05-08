using JetBrains.Annotations;

namespace Cts.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static DevSettingsSection DevSettings { get; set; } = new();

    // PROD configuration settings
    public static readonly DevSettingsSection ProductionDefault = new()
    {
        UseDevSettings = false,
        BuildDatabase = true,
        UseEfMigrations = true,
        EnableTestUser = false,
        TestUserIsAuthenticated = false,
        TestUserRoles = [],
        UseSecurityHeadersInDev = false,
        EnableWebOptimizerInDev = false,
    };

    // DEV configuration settings
    public record DevSettingsSection
    {
        /// <summary>
        /// Enable (`true`) or disable (`false`) the development settings.
        /// </summary>
        public bool UseDevSettings { get; [UsedImplicitly] init; }

        /// <summary>
        /// Build a SQL Server database (`true`) or use an in-memory data store (`false`).
        /// </summary>
        public bool BuildDatabase { get; [UsedImplicitly] init; }

        /// <summary>
        /// Create a database using Entity Framework migrations (`true`) or solely based on the `DbContext` (`false`).
        /// (Only applies if <see cref="BuildDatabase"/> is `true`.)
        /// </summary>
        public bool UseEfMigrations { get; [UsedImplicitly] init; }

        /// <summary>
        /// Enable a test user for development (`true`) or disable (`false`).
        /// </summary>
        public bool EnableTestUser { get; [UsedImplicitly] init; }

        /// <summary>
        /// Simulate a successful login with a test account (`true`) or simulate a failed login (`false`).
        /// (Only applies if <see cref="EnableTestUser"/> is `false`.)
        /// </summary>
        public bool TestUserIsAuthenticated { get; [UsedImplicitly] init; }

        /// <summary>
        /// Add listed Roles to the test user account.
        /// (Only applies if <see cref="EnableTestUser"/> is `false` and <see cref="TestUserIsAuthenticated"/> is `true`.)
        /// </summary>
        public string[] TestUserRoles { get; [UsedImplicitly] init; } = [];

        /// <summary>
        /// Include HTTP security headers when running in a Development environment (`true`).
        /// </summary>
        public bool UseSecurityHeadersInDev { get; [UsedImplicitly] init; }

        /// <summary>
        /// Use WebOptimizer to bundle and minify CSS and JS files (`true`).
        /// </summary>
        public bool EnableWebOptimizerInDev { get; [UsedImplicitly] init; }
    }
}
