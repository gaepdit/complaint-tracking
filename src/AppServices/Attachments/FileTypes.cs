using System.Diagnostics.CodeAnalysis;

namespace Cts.AppServices.Attachments;

public static class FileTypes
{
    internal static readonly IEnumerable<string> AllowedFileTypes =
    [
        ".csv", ".docx", ".gif", ".htm", ".html", ".jpeg", ".jpg", ".markdown", ".md", ".pdf", ".png", ".pptx", ".rtf",
        ".svg", ".txt", ".xlsx",
    ];

    private static readonly IEnumerable<string> ImageFileTypes = [".gif", ".jpeg", ".jpg", ".png"];

    internal static readonly IEnumerable<string> TextFileTypes =
        [".csv", ".htm", ".html", ".markdown", ".md", ".svg", ".txt"];

    public static string FileTypesAcceptString { get; } = string.Join(",", AllowedFileTypes);

    internal static bool FileNameImpliesImage(string fileName) =>
        ImageFileTypes.Contains(Path.GetExtension(fileName).ToLowerInvariant());

    public static string GetContentType(string extension) =>
        FileContentTypes.GetValueOrDefault(extension.ToLowerInvariant(), "application/octet-stream");

    public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const string ZipContentType = "application/zip";

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
        { ".xlsx", ExcelContentType },
        { ".zip", ZipContentType },
    };
}
