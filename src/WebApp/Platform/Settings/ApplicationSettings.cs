using JetBrains.Annotations;

namespace Cts.WebApp.Platform.Settings;

public static class ApplicationSettings
{
    public static LocalDev LocalDevSettings { get; } = new();

    public class LocalDev
    {
        public bool AuthenticatedUser { get; [UsedImplicitly] init; }
        public bool BuildLocalDb { get; [UsedImplicitly] init; }
        public bool UseEfMigrations { get; [UsedImplicitly] init; }
    }
}
