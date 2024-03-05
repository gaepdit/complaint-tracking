namespace Cts.WebApp.Platform.Constants;

// App-wide global variables
internal static class GlobalConstants
{
    // Default pagination size for search results, etc.
    public const int PageSize = 25;

    // Image thumbnail size
    public const int ThumbnailSize = 90;

    // Attachment file paths
    public const string AttachmentsFolder = "UserFiles/Attachments";
    public const string ThumbnailsFolder = "UserFiles/Thumbnails";

    // Data export
    public const string ExportFolder = "DataExport";
    public const int ExportLifespan = 15; // hours
}
