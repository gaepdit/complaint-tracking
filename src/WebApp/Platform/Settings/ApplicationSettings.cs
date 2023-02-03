using JetBrains.Annotations;

namespace Cts.WebApp.Platform.Settings;

public static class ApplicationSettings
{
    public static LocalDev LocalDevSettings { get; } = new();

    public class LocalDev
    {
        public bool UseAzureAd { get; [UsedImplicitly] init; }
        public bool LocalUserIsAuthenticated { get; [UsedImplicitly] init; }
        public bool LocalUserIsAdmin { get; [UsedImplicitly] init; }
        public bool UseInMemoryData { get; [UsedImplicitly] init; } = true;
        public bool UseEfMigrations { get; [UsedImplicitly] init; }
    }

    // Raygun client settings
    public static RaygunClientSettings RaygunSettings { get; } = new();

    public class RaygunClientSettings
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }
}
