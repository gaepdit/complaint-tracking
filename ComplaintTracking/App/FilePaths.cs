namespace ComplaintTracking
{
    // File system folder names
    internal static class FilePaths
    {
        public static string AttachmentsFolder { get; } = Path.Combine("UserFiles", "Attachments");
        public static string ThumbnailsFolder { get; } = Path.Combine("UserFiles", "Thumbnails");
        public static string ExportFolder { get; } = "DataExport";
        public static string UnsentEmailFolder { get; } = "UnsentEmail";
    }
}
