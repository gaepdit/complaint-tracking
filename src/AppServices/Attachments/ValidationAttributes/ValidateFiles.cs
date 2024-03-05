using Microsoft.AspNetCore.Http;

namespace Cts.AppServices.Attachments.ValidationAttributes;

public static class ValidateFiles
{
    public static ValidateFilesResult Validate(this List<IFormFile> formFiles,int maxNumberOfFiles)
    {
        var messages = new List<string>();

        // FilesNotEmpty
        if (!formFiles.TrueForAll(FileIsNotEmpty)) 
            messages.Add(EmptyFileErrorMessage);

        // ValidateFileTypes
        if (!formFiles.WithinMaxNumberOfFiles(maxNumberOfFiles)) 
            messages.Add(TooManyFilesErrorMessage(maxNumberOfFiles));
        
        // ValidateFileTypes
        if (!formFiles.TrueForAll(IsFileSignatureValid)) 
            messages.Add(InvalidFileTypeErrorMessage);
        
        return messages.Count == 0 ? ValidateFilesResult.Valid : ValidateFilesResult.Invalid(messages);
    }

    // FilesNotEmpty
    public static bool FileIsNotEmpty(this IFormFile formFile) => formFile.Length > 0;
    public const string EmptyFileErrorMessage = "Empty file selected.";

    // MaxNumberOfFiles
    public static bool WithinMaxNumberOfFiles(this List<IFormFile> formFiles, int maxNumberOfFiles) => 
        formFiles.Count <= maxNumberOfFiles;
    public static string TooManyFilesErrorMessage(int maxNumberOfFiles) =>
        $"No more than {maxNumberOfFiles} files may be uploaded at a time.";

    // ValidateFileTypes
    public const string InvalidFileTypeErrorMessage = "Invalid file type selected.";
    public static bool IsFileSignatureValid(this IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!FileTypes.AllowedFileTypes.Contains(ext)) return false;
        if (FileTypes.TextFileTypes.Contains(ext)) return true;

        var signatureList = FileSignatures[ext];
        using var reader = new BinaryReader(file.OpenReadStream());
        var headerBytes = reader.ReadBytes(signatureList.Max(n => n.Length));
        return signatureList.Exists(s => headerBytes.Take(s.Length).SequenceEqual(s));
    }

    // [List of file signatures - Wikipedia](https://en.wikipedia.org/wiki/List_of_file_signatures)
    private static List<byte[]> GifSignatures { get; } =
        [[0x47, 0x49, 0x46, 0x38, 0x37, 0x61], [0x47, 0x49, 0x46, 0x38, 0x39, 0x61]];

    private static List<byte[]> JpegSignatures { get; } =
        [[0xFF, 0xD8, 0xFF, 0xE0], [0xFF, 0xD8, 0xFF, 0xDB], [0xFF, 0xD8, 0xFF, 0xEE], [0xFF, 0xD8, 0xFF, 0xE1]];

    private static List<byte[]> PdfSignatures { get; } = [[0x25, 0x50, 0x44, 0x46, 0x2D]];
    private static List<byte[]> PngSignatures { get; } = [[0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]];
    private static List<byte[]> RtfSignatures { get; } = [[0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31]];
    private static List<byte[]> ZipSignatures { get; } = [[0x50, 0x4B, 0x03, 0x04]];

    private static readonly Dictionary<string, List<byte[]>> FileSignatures = new()
    {
        { ".docx", ZipSignatures },
        { ".gif", GifSignatures },
        { ".jpeg", JpegSignatures },
        { ".jpg", JpegSignatures },
        { ".pdf", PdfSignatures },
        { ".png", PngSignatures },
        { ".pptx", ZipSignatures },
        { ".rtf", RtfSignatures },
        { ".xlsx", ZipSignatures },
    };

#pragma warning disable S125 // Sections of code should not be commented out
    // In case MS Office files (.doc, .xls, .ppt) are allowed in the future.
    // private static List<byte[]> MsOfficeSignatures { get; } = [[0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1]];
#pragma warning restore S125
}
