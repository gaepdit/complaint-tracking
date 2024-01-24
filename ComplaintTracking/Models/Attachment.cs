using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.Models
{
    public class Attachment
    {
        public Guid Id { get; set; }

        public virtual Complaint Complaint { get; set; }
        public int ComplaintId { get; set; }

        [StringLength(245)]
        public string FileName { get; set; }

        [StringLength(10)]
        public string FileExtension { get; set; }

        public long Size { get; set; }

        public DateTime DateUploaded { get; set; }

        public virtual ApplicationUser UploadedBy { get; set; }
        public string UploadedById { get; set; }

        public bool IsImage { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DateDeleted { get; set; }

        public virtual ApplicationUser DeletedBy { get; set; }
        public string DeletedById { get; set; }

        public string FileId => string.Concat(Id, FileExtension);
    }
}
