using Cts.Domain.Entities.Complaints;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.Attachments;

public class Attachment : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Attachment() { }

    internal Attachment(Guid id) : base(id) { }

    // Properties

    public required Complaint Complaint { get; init; }

    [StringLength(245)]
    public required string FileName { get; init; } = string.Empty;

    [StringLength(10)]
    public required string FileExtension { get; init; } = string.Empty;

    public string FileId => $"{Id}{FileExtension}";

    public required long Size { get; init; }

    public DateTimeOffset UploadedDate { get; init; } = DateTimeOffset.Now;

    public ApplicationUser? UploadedBy { get; init; }

    public bool IsImage { get; set; }
}
