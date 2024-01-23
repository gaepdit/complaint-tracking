using ComplaintTracking.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.ViewModels
{
    public class AttachmentViewModel
    {
        public AttachmentViewModel() { }

        public AttachmentViewModel(Attachment a)
        {
            if (a == null) return;
            
            Id = a.Id;
            ComplaintId = a.ComplaintId;
            FileName = a.FileName;
            FileExtension = a.FileExtension;
            Size = a.Size;
            DateUploaded = a.DateUploaded;
            UploadedBy = a.UploadedBy;
            IsImage = a.IsImage;
        }

        public Guid Id { get; set; }

        [Display(Name = "Complaint ID")]
        public int ComplaintId { get; set; }

        [Display(Name = "File")]
        public string FileName { get; set; }

        [UIHint("FileTypeIcon")]
        public string FileExtension { get; set; }

        [Display(Name = "Size")]
        [UIHint("FileSize")]
        public long Size { get; set; }

        [Display(Name = "Date uploaded")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeShortDisplay)]
        public DateTime DateUploaded { get; set; }

        [Display(Name = "Uploaded by")]
        public ApplicationUser UploadedBy { get; set; }

        public bool IsImage { get; set; }

        public string FileId => string.Concat(Id, FileExtension);
    }
}
