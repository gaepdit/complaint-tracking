using JetBrains.Annotations;

namespace Cts.WebApp.Platform.Settings;

internal static class ApplicationSettings
{
    public static DevSettingsSection DevSettings { get; set; } = new();

    public static readonly DevSettingsSection ProductionDefault = new()
    {
        UseInMemoryData = false,
        UseEfMigrations = true,
        UseAzureAd = true,
        LocalUserIsAuthenticated = false,
        LocalUserIsStaff = false,
        LocalUserIsAdmin = false,
        UseInMemoryFiles = false,
    };

    public class DevSettingsSection
    {
        /// <summary>
        /// Uses in-memory data when `true`. Connects to a SQL Server database when `false`.
        /// </summary>
        public bool UseInMemoryData { get; [UsedImplicitly] init; }

        /// <summary>
        /// Uses Entity Framework migrations when `true`. When set to `false`, the database is deleted and
        /// recreated on each run. (Only applies if <see cref="UseInMemoryData"/> is `false`.)
        /// </summary>
        public bool UseEfMigrations { get; [UsedImplicitly] init; }

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
        /// Simulates a successful login with a test account with the Staff role when `true`. When `false`, no
        /// roles are added.
        /// (Only applies if <see cref="LocalUserIsAuthenticated"/> is `true`.)
        /// </summary>
        public bool LocalUserIsStaff { get; [UsedImplicitly] init; }

        /// <summary>
        /// Adds all App Roles to the logged in account when `true` or no roles when `false`. (Applies whether
        /// <see cref="UseAzureAd"/> is `true` or `false`.)
        /// </summary>
        public bool LocalUserIsAdmin { get; [UsedImplicitly] init; }

        /// <summary>
        /// If `true`, files are seeded from the TestData project and stored in memory. If `false`, attachment
        /// files are saved/loaded from the file system.
        /// </summary>
        public bool UseInMemoryFiles { get; [UsedImplicitly] init; }
    }

    // Raygun client settings
    public static RaygunClientSettings RaygunSettings { get; } = new();

    public class RaygunClientSettings
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }
}
