namespace Cts.Domain.Entities;

public class Attachment
{
    public Guid Id { get; set; }

    public virtual Complaint Complaint { get; set; } = null!;
    public int ComplaintId { get; set; }

    [StringLength(245)]
    public string FileName { get; set; } = string.Empty;

    [StringLength(10)]
    public string FileExtension { get; set; } = string.Empty;

    public long Size { get; set; }

    public DateTime DateUploaded { get; set; }

    public virtual ApplicationUser? UploadedBy { get; set; }
    public string? UploadedById { get; set; }

    public bool IsImage { get; set; }

    public bool Deleted { get; set; }

    public DateTime? DateDeleted { get; set; }

    public virtual ApplicationUser? DeletedBy { get; set; }
    public string? DeletedById { get; set; }

#pragma warning disable S125
    // public string FilePath => Path.Combine(FilePaths.AttachmentsFolder, Id + FileExtension);
    //
    // public string ThumbnailPath => IsImage ? Path.Combine(FilePaths.ThumbnailsFolder, Id + FileExtension) : null;
#pragma warning restore S125
}
