using Cts.Domain.ActionTypes;
using Cts.Domain.Identity;
using JetBrains.Annotations;

namespace Cts.Domain.ComplaintActions;

public class ComplaintAction : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private ComplaintAction() { }

    internal ComplaintAction(Guid id) : base(id) { }

    // Properties

    public int ComplaintId { get; init; }

    public DateTime ActionDate { get; init; }

    public ActionType ActionType { get; init; } = null!;

    [StringLength(100)]
    public string? Investigator { get; init; }

    public DateTimeOffset DateEntered { get; init; }

    public ApplicationUser EnteredBy { get; set; } = null!;

    public string? Comments { get; set; }
}
