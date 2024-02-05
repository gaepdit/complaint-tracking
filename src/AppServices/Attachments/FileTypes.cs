using System.Diagnostics.CodeAnalysis;

namespace Cts.AppServices.Attachments;

public static class FileTypes
{
    public static bool FileUploadAllowed(string fileName) =>
        Path.GetExtension(fileName).ToLower() is ".csv" or ".doc" or ".docx" or ".gif" or ".htm" or ".html" or ".jpeg"
            or ".jpg" or ".pdf" or ".png" or ".ppt" or ".pptx" or ".rtf" or ".svg" or ".txt" or ".xls" or ".xlsx";

    public static bool FileNameImpliesImage(string fileName) => 
        FileExtensionImpliesImage(Path.GetExtension(fileName));

    public static bool FileExtensionImpliesImage(string extension) =>
        extension.ToLower() is ".bmp" or ".gif" or ".jpeg" or ".jpg" or ".png";

    public static string GetContentType(string extension) =>
        FileContentTypes.GetValueOrDefault(extension.ToLower(), "application/octet-stream");

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    private static readonly Dictionary<string, string> FileContentTypes = new()
    {
        { ".bmp", "image/bmp" },
        { ".csv", "text/csv" },
        { ".doc", "application/msword" },
        { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { ".gif", "image/gif" },
        { ".htm", "text/html" },
        { ".html", "text/html" },
        { ".jpeg", "image/jpeg" },
        { ".jpg", "image/jpeg" },
        { ".pdf", "application/pdf" },
        { ".png", "image/png" },
        { ".ppt", "application/vnd.ms-powerpoint" },
        { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
        { ".rtf", "application/rtf" },
        { ".svg", "image/svg+xml" },
        { ".txt", "text/plain" },
        { ".xls", "application/ms-excel" },
        { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { ".zip", "application/zip" },
    };
}
