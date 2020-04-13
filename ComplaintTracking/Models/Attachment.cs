using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

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

        public bool IsImage { get; set; } = false;
        
        public bool Deleted { get; set; } = false;

        public DateTime? DateDeleted { get; set; }

        public virtual ApplicationUser DeletedBy { get; set; }
        public string DeletedById { get; set; }

        public string FilePath
        {
            get
            {
                return Path.Combine(FilePaths.AttachmentsFolder, Id + FileExtension);
            }
        }

        public string ThumbnailPath
        {
            get
            {
                return IsImage ? null : Path.Combine(FilePaths.ThumbnailsFolder, Id + FileExtension);
            }
        }
    }
}