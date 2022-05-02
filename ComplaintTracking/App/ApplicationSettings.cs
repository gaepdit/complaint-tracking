namespace ComplaintTracking.App;

internal static class ApplicationSettings
{
    public const string RaygunSettingsSection = "RaygunSettings";
    public static RaygunSettings Raygun { get; } = new();
}

internal class RaygunSettings
{
    public string ApiKey { get; init; }
}
