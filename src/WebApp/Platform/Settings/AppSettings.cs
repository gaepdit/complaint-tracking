using Cts.AppServices.Attachments;
using Cts.WebApp.Platform.Constants;
using JetBrains.Annotations;
using System.Reflection;

namespace Cts.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    // Support settings
    public static string Version { get; } = GetVersion();
    public static Support SupportSettings { get; } = new();
    public static Raygun RaygunSettings { get; } = new();
    public static string? OrgNotificationsApiUrl { get; set; }

    public record Support
    {
        public string? CustomerSupportEmail { get; [UsedImplicitly] init; }
        public string? TechnicalSupportEmail { get; [UsedImplicitly] init; }
        public string? TechnicalSupportSite { get; [UsedImplicitly] init; }
    }

    public record Raygun
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }

    // Attachment File Service configuration
    public static IAttachmentService.AttachmentServiceConfig AttachmentServiceConfig { get; } =
        new(GlobalConstants.AttachmentsFolder, GlobalConstants.ThumbnailsFolder, GlobalConstants.ThumbnailSize);

    private static string GetVersion()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var segments = (entryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? entryAssembly?.GetName().Version?.ToString() ?? "").Split('+');
        return segments[0] + (segments.Length > 0 ? $"+{segments[1][..Math.Min(7, segments[1].Length)]}" : "");
    }
}
