using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.ComplaintActions;

public class ComplaintAction : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private ComplaintAction() { }

    internal ComplaintAction(Guid id, Complaint complaint, ActionType actionType) : base(id)
    {
        Complaint = complaint;
        ComplaintId = complaint.Id;
        ActionType = actionType;
    }

    // Properties

    public int ComplaintId { get; init; }
    public Complaint Complaint { get; private init; } = null!;

    public ActionType ActionType { get; set; } = null!;

    public DateOnly ActionDate { get; set; }

    [StringLength(100)]
    public string Investigator { get; set; } = string.Empty;

    [StringLength(10_000)]
    public string Comments { get; set; } = string.Empty;

    public DateTimeOffset? EnteredDate { get; init; } = DateTimeOffset.Now;

    public ApplicationUser? EnteredBy { get; init; }

    // Properties: Deletion

    public ApplicationUser? DeletedBy { get; set; }
}
