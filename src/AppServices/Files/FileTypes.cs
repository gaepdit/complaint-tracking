using System.Diagnostics.CodeAnalysis;

namespace Cts.AppServices.Files;

public static class FileTypes
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    private static readonly Dictionary<string, string> FileContentTypes = new()
    {
        { ".pdf", "application/pdf" },
        { ".txt", "text/plain" },
        { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { ".xls", "application/ms-excel" },
        { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { ".doc", "application/msword" },
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".png", "image/png" },
        { ".bmp", "image/bmp" },
        { ".gif", "image/gif" },
        { ".svg", "image/svg+xml" },
        { ".html", "text/html" },
        { ".htm", "text/html" },
        { ".rtf", "application/rtf" },
        { ".zip", "application/zip" },
        { ".csv", "text/csv" },
        { ".ppt", "application/vnd.ms-powerpoint" },
        { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
    };

    public static string GetContentType(string fileExtension) =>
        FileContentTypes.ContainsKey(fileExtension.ToLower())
            ? FileContentTypes[fileExtension.ToLower()]
            : "application/octet-stream";

    public static bool FilenameImpliesImage(string fileExtension) =>
        fileExtension.ToLower() is ".bmp" or ".jpg" or ".jpeg" or ".png" or ".gif" or ".svg";
}
