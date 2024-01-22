namespace ComplaintTracking
{
    // File system folder names
    internal static class FilePaths
    {
        // TODO
        public static string AttachmentsFolder { get; } = Path.Combine("UserFiles", "Attachments");
        // TODO
        public static string ExportFolder { get; } = "DataExport";
        // TODO
        public static string ThumbnailsFolder { get; } = Path.Combine("UserFiles", "Thumbnails");
        // Done
        public static string UnsentEmailFolder { get; } = "UnsentEmail";
    }
}
