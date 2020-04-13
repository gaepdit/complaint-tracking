using ComplaintTracking.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace ComplaintTracking.Models
{
    public class ComplaintTransition : IAuditable
    {
        public Guid Id { get; set; }

        [Required]
        public virtual Complaint Complaint { get; set; }
        public int ComplaintId { get; set; }

        [Display(Name = "Transferred By")]
        public virtual ApplicationUser TransferredByUser { get; set; }
        public string TransferredByUserId { get; set; }

        [Display(Name = "From")]
        public virtual ApplicationUser TransferredFromUser { get; set; }
        public string TransferredFromUserId { get; set; } = null;

        public virtual Office TransferredFromOffice { get; set; }
        public Guid? TransferredFromOfficeId { get; set; } = null;

        [Display(Name = "To")]
        public virtual ApplicationUser TransferredToUser { get; set; }
        public string TransferredToUserId { get; set; } = null;

        public virtual Office TransferredToOffice { get; set; }
        public Guid? TransferredToOfficeId { get; set; } = null;

        [Display(Name = "Date Transferred")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeDisplay)]
        public DateTime DateTransferred { get; set; } = DateTime.Now;

        [Display(Name = "Date Accepted")]
        [DisplayFormat(DataFormatString = CTS.FormatDateTimeDisplay)]
        public DateTime? DateAccepted { get; set; } = null;

        [Display(Name = "Action")]
        public TransitionType TransitionType { get; set; }

        [Display(Name = "Comment")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000)]
        public string Comment { get; set; } = null;
    }

    public enum TransitionType
    {
        New = 0,
        Assigned = 1,
        [Display(Name = "Submitted For Review")]
        SubmittedForReview = 2,
        [Display(Name = "Returned By Reviewer")]
        ReturnedByReviewer = 3,
        Closed = 4,
        Reopened = 5,
        Deleted = 6,
        Restored = 7
    }
}
