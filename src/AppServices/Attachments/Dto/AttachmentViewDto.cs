using Cts.AppServices.Utilities;
using GaEpd.AppLibrary.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cts.AppServices.Attachments.Dto;

public record AttachmentViewDto
{
    public Guid Id { get; init; }

    public string FileName { get; init; } = string.Empty;

    [JsonIgnore]
    public string FileExtension { get; init; } = string.Empty;

    [JsonIgnore]
    public string FileId => string.Concat(Id.ToString(), FileExtension);

    [Display(Name = "Size in bytes")]
    public long Size { get; init; }

    public string SizeDescription => FileSize.ToFileSizeString(Size);

    [Display(Name = "Uploaded By")]
    public string UploadedByName => new[] { UploadedByGivenName, UploadedByFamilyName }.ConcatWithSeparator();

    public string? UploadedByGivenName { get; init; }
    public string? UploadedByFamilyName { get; init; }
    public DateTimeOffset UploadedDate { get; init; }

    public bool IsImage { get; init; }
    public bool IsForPublic { get; set; }
    public bool IsDeleted { get; init; }
}
