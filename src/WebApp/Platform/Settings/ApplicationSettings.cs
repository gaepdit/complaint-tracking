using JetBrains.Annotations;

namespace MyAppRoot.WebApp.Platform.Settings;

public static class ApplicationSettings
{
    public static LocalDev LocalDevSettings { get; } = new();

    public class LocalDev
    {
        public bool AuthenticatedUser { get; [UsedImplicitly] init; }
        public bool AuthenticatedUserIsAdmin { get; [UsedImplicitly] init; }
        public bool BuildLocalDb { get; [UsedImplicitly] init; }
        public bool UseEfMigrations { get; [UsedImplicitly] init; }
    }

    // Raygun client settings
    public static RaygunClientSettings RaygunSettings { get; } = new();

    public class RaygunClientSettings
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }
}
