namespace ComplaintTracking.App;

internal static class ApplicationSettings
{
    public static RaygunClientSettings RaygunSettings { get; } = new();
}

internal class RaygunClientSettings
{
    public string ApiKey { get; init; }
}
