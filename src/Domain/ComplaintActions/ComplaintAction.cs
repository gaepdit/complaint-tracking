using Cts.Domain.ActionTypes;
using Cts.Domain.Complaints;
using Cts.Domain.Identity;

namespace Cts.Domain.ComplaintActions;

public class ComplaintAction : AuditableSoftDeleteEntity
{
    public Complaint Complaint { get; set; } = null!;
    public int ComplaintId { get; set; }

    public DateTime ActionDate { get; set; }

    public ActionType ActionType { get; set; } = null!;
    public Guid ActionTypeId { get; set; }

    [StringLength(100)]
    public string? Investigator { get; set; }

    public DateTime? DateEntered { get; set; }

    public ApplicationUser EnteredBy { get; set; } = null!;

    public string? Comments { get; set; }
}
