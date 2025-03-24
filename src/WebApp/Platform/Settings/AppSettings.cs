using Cts.AppServices.Attachments;
using Cts.WebApp.Platform.Constants;
using JetBrains.Annotations;

namespace Cts.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    // Support settings
    public static SupportSettingsSection SupportSettings { get; } = new();

    public record SupportSettingsSection
    {
        public string? CustomerSupportEmail { get; [UsedImplicitly] init; }
        public string? TechnicalSupportEmail { get; [UsedImplicitly] init; }
        public string? TechnicalSupportSite { get; [UsedImplicitly] init; }
        public string? InformationalVersion { get; set; }
        public string? InformationalBuild { get; set; }
    }

    // Organizational notifications
    public static string? OrgNotificationsApiUrl { get; set; }

    // Raygun client settings
    public static RaygunClientSettings RaygunSettings { get; } = new();

    public record RaygunClientSettings
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
        public bool ExcludeErrorsFromLocal { get; [UsedImplicitly] init; }
    }

    // Attachment File Service configuration
    public static IAttachmentService.AttachmentServiceConfig AttachmentServiceConfig { get; } =
        new(GlobalConstants.AttachmentsFolder, GlobalConstants.ThumbnailsFolder, GlobalConstants.ThumbnailSize);
}
