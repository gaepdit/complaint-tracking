using System.Diagnostics.CodeAnalysis;

namespace Cts.AppServices.Attachments;

public static class FileTypes
{
    private static string[] AllowedFileTypes { get; } =
    [
        ".csv", ".docx", ".gif", ".htm", ".html", ".jpeg", ".jpg", ".markdown", ".md", ".pdf", ".png", ".pptx", ".rtf",
        ".svg", ".txt", ".xlsx",
    ];

    public static string AcceptFileTypes { get; } = string.Join(",", AllowedFileTypes);

    public static bool FileUploadAllowed(string fileName) =>
        AllowedFileTypes.Contains(Path.GetExtension(fileName).ToLower());

    public static bool FileNameImpliesImage(string fileName) =>
        Path.GetExtension(fileName).ToLower() is ".gif" or ".jpeg" or ".jpg" or ".png";

    public static string GetContentType(string extension) =>
        FileContentTypes.GetValueOrDefault(extension.ToLower(), "application/octet-stream");

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    // This list contains file types that are no longer allowed but may exist for previous uploads. 
    private static readonly Dictionary<string, string> FileContentTypes = new()
    {
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
