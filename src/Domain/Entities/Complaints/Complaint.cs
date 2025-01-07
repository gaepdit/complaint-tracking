using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using System.Text.Json.Serialization;

namespace Cts.Domain.Entities.Complaints;

public class Complaint : AuditableSoftDeleteEntity<int>
{
    // Constants


    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Complaint() { }

    internal Complaint(int? id)
    {
        if (id is not null) Id = id.Value;
    }

    // Properties

    // Properties: Status & meta-data

    [StringLength(25)]
    public ComplaintStatus Status { get; internal set; } = ComplaintStatus.New;

    public DateTimeOffset EnteredDate { get; init; } = DateTimeOffset.Now;

    public ApplicationUser? EnteredBy { get; init; }

    public DateTimeOffset ReceivedDate { get; set; }

    public ApplicationUser? ReceivedBy { get; set; }

    // Properties: Caller

    [StringLength(150)]
    public string? CallerName { get; set; }

    [StringLength(150)]
    public string? CallerRepresents { get; set; }

    public IncompleteAddress? CallerAddress { get; set; }

    public PhoneNumber? CallerPhoneNumber { get; set; }
    public PhoneNumber? CallerSecondaryPhoneNumber { get; set; }
    public PhoneNumber? CallerTertiaryPhoneNumber { get; set; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string? CallerEmail { get; set; }

    // Properties: Complaint details

    [StringLength(15_000)]
    public string? ComplaintNature { get; set; }

    [StringLength(2000)]
    public string? ComplaintLocation { get; set; }

    [StringLength(2600)]
    public string? ComplaintDirections { get; set; }

    [StringLength(50)]
    public string? ComplaintCity { get; set; }

    [StringLength(15)]
    public string? ComplaintCounty { get; set; }

    public Concern PrimaryConcern { get; set; } = null!;

    public Concern? SecondaryConcern { get; set; }

    // Properties: Source

    [StringLength(50)]
    public string? SourceFacilityIdNumber { get; set; }

    [StringLength(100)]
    public string? SourceFacilityName { get; set; }

    [StringLength(100)]
    public string? SourceContactName { get; set; }

    public IncompleteAddress? SourceAddress { get; set; }

    public PhoneNumber? SourcePhoneNumber { get; set; }
    public PhoneNumber? SourceSecondaryPhoneNumber { get; set; }
    public PhoneNumber? SourceTertiaryPhoneNumber { get; set; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string? SourceEmail { get; set; }

    // Properties: Assignment/History

    public Office CurrentOffice { get; internal set; } = null!;

    public ApplicationUser? CurrentOwner { get; internal set; }

    public DateTimeOffset? CurrentOwnerAssignedDate { get; internal set; }

    public DateTimeOffset? CurrentOwnerAcceptedDate { get; internal set; }

    // Properties: Transitions
    public List<ComplaintTransition> ComplaintTransitions { get; } = [];

    // Properties: Actions
    public List<ComplaintAction> Actions { get; } = [];

    // Properties: Attachments
    public List<Attachment> Attachments { get; } = [];

    // Properties: Review/Closure

    public ApplicationUser? ReviewedBy { get; internal set; }

    [StringLength(7000)]
    public string? ReviewComments { get; internal set; }

    public bool ComplaintClosed { get; internal set; }

    public DateTimeOffset? ComplaintClosedDate { get; internal set; }

    // Properties: Deletion

    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }
}

// Enums

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ComplaintStatus
{
    /// <summary>
    /// Represents a new complaint that has not been accepted.
    /// </summary>
    New = 0,

    /// <summary>
    ///  Represents a new complaint that has been accepted.
    /// </summary>
    [Display(Name = "Under Investigation")]
    UnderInvestigation = 1,

    /// <summary>
    ///  Represents a complaint that has been submitted for review.
    /// </summary>
    [Display(Name = "Review Pending")] ReviewPending = 2,

    /// <summary>
    ///  Represents a complaint that has been approved/closed by a reviewer.
    /// </summary>
    [Display(Name = "Approved/Closed")] Closed = 3,

    /// <summary>
    ///  Represents a complaint that has been administratively closed (e.g., by EPD-IT).
    /// </summary>
    [Display(Name = "Administratively Closed")]
    AdministrativelyClosed = 4,
}
