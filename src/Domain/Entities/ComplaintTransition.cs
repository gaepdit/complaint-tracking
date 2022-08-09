using Cts.Domain.Entities.BaseEntities;
using Cts.Domain.Users;

namespace Cts.Domain.Entities;

public class ComplaintTransition : AuditableEntity
{
    [Required]
    public Complaint Complaint { get; set; } = null!;

    public int ComplaintId { get; set; }

    public ApplicationUser? TransferredByUser { get; set; }

    public ApplicationUser? TransferredFromUser { get; set; }

    public Office? TransferredFromOffice { get; set; }
    public Guid? TransferredFromOfficeId { get; set; }

    public ApplicationUser? TransferredToUser { get; set; }

    public Office? TransferredToOffice { get; set; }
    public Guid? TransferredToOfficeId { get; set; } = null;

    public DateTime DateTransferred { get; set; } = DateTime.Now;

    public DateTime? DateAccepted { get; set; } = null;

    public TransitionType TransitionType { get; set; } = TransitionType.New;

    [StringLength(4000)]
    public string? Comment { get; set; }
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
    Restored = 7,
}
