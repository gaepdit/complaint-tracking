namespace ComplaintTracking
{
    public static class FileTypes
    {
        // Special content types
        public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string ZipContentType = "application/zip";

        public static bool FileUploadAllowed(string fileName) =>
            Path.GetExtension(fileName).ToLower() switch
            {
                ".txt" or ".jpg" or ".jpeg" or ".png" or ".gif" or ".svg" or ".pdf" or ".html" or ".htm" or
                ".csv" or ".xlsx" or ".xls" or ".rtf" or ".docx" or ".doc" or ".ppt" or ".pptx" => true,
                _ => false,
            };

        public static bool FilenameImpliesImage(string fileName) =>
            Path.GetExtension(fileName).ToLower() switch
            {
                ".bmp" or ".jpg" or ".jpeg" or ".png" or ".gif" => true,
                _ => false,
            };

        public static string GetContentType(string fileName) =>
            Path.GetExtension(fileName).ToLower() switch
            {
                ".txt" => "text/plain",
                ".xlsx" => ExcelContentType,
                ".xls" => "application/ms-excel",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".doc" => "application/msword",
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp",
                ".gif" => "image/gif",
                ".svg" => "image/svg+xml",
                ".html" or ".htm" => "text/html",
                ".rtf" => "application/rtf",
                ".zip" => ZipContentType,
                ".csv" => "text/csv",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                _ => "application/octet-stream",
            };
    }
}
