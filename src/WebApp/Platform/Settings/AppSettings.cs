using Cts.AppServices.Attachments;
using Cts.WebApp.Platform.Constants;
using JetBrains.Annotations;

namespace Cts.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    // Support settings
    public static string? Version { get; private set; }
    public static Support SupportSettings { get; } = new();
    public static Raygun RaygunSettings { get; } = new();
    public static string? OrgNotificationsApiUrl { get; private set; }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record Support
    {
        public string? CustomerSupportEmail { get; init; }
        public string? TechnicalSupportEmail { get; init; }
        public string? TechnicalSupportSite { get; init; }
    }

    public record Raygun
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }

    // Attachment File Service configuration
    public static IAttachmentService.AttachmentServiceConfig AttachmentServiceConfig { get; } =
        new(GlobalConstants.AttachmentsFolder, GlobalConstants.ThumbnailsFolder, GlobalConstants.ThumbnailSize);
}
