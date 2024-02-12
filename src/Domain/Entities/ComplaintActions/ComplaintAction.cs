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
        ActionType = actionType;
    }

    // Properties

    public Complaint Complaint { get; private init; } = default!;

    public ActionType ActionType { get; set; } = default!;

    public DateOnly ActionDate { get; set; }

    [StringLength(100)]
    public string Investigator { get; set; } = string.Empty;

    public string Comments { get; set; } = string.Empty;

    public DateTimeOffset EnteredDate { get; init; } = DateTimeOffset.Now;

    public ApplicationUser? EnteredBy { get; init; }
}
