using System.Text.Json.Serialization;

namespace Cts.AppServices.Attachments;

public class AttachmentPublicViewDto
{
    public Guid Id { get; init; }

    [JsonIgnore]
    public int ComplaintId { get; init; }

    public string FileName { get; init; } = string.Empty;

    public string FileExtension { get; init; } = string.Empty;

    public long Size { get; init; }

    public DateTimeOffset DateUploaded { get; init; }

    public bool IsImage { get; init; }

    public string AttachmentFileName => string.Concat(Id.ToString(), FileExtension);
}
