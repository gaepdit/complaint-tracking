using Cts.AppServices.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cts.AppServices.Attachments;

public class AttachmentPublicViewDto
{
    public Guid Id { get; init; }
    public string FileName { get; init; } = string.Empty;

    [JsonIgnore]
    public string FileExtension { get; init; } = string.Empty;

    [Display(Name = "Size in bytes")]
    public long Size { get; init; }

    public string SizeDescription => FileSize.ToFileSizeString(Size);

    public DateTimeOffset DateUploaded { get; init; }
    public bool IsImage { get; init; }

    [JsonIgnore]
    public string AttachmentFileName => string.Concat(Id.ToString(), FileExtension);
}
