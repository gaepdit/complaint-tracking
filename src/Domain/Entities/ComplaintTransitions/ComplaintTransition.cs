using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using JetBrains.Annotations;
using System.Text.Json.Serialization;

namespace Cts.Domain.Entities.ComplaintTransitions;

public class ComplaintTransition : AuditableEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private ComplaintTransition() { }

    internal ComplaintTransition(Guid id) : base(id) { }

    // Properties

    public Complaint Complaint { get; init; } = default!;

    public TransitionType TransitionType { get; init; } = TransitionType.New;

    public DateTimeOffset TransferredDate { get; init; } = DateTimeOffset.Now;

    public ApplicationUser? TransferredByUser { get; init; }

    public ApplicationUser? TransferredFromUser { get; init; }

    public Office? TransferredFromOffice { get; init; }

    public ApplicationUser? TransferredToUser { get; init; }

    public Office? TransferredToOffice { get; init; }

    public DateTimeOffset? AcceptedDate { get; init; }

    [StringLength(4000)]
    public string? Comment { get; init; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransitionType
{
    [Display(Name = "New")]
    New = 0,

    [Display(Name = "Assigned")]
    Assigned = 1,

    [Display(Name = "Submitted For Review")]
    SubmittedForReview = 2,

    [Display(Name = "Returned By Reviewer")]
    ReturnedByReviewer = 3,

    [Display(Name = "Closed")]
    Closed = 4,

    [Display(Name = "Reopened")]
    Reopened = 5,

    [Display(Name = "Deleted")]
    Deleted = 6,

    [Display(Name = "Restored")]
    Restored = 7,
}
