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

    internal Complaint() { }

    internal Complaint(int id) : base(id) { }

    // Methods

    internal void SetId(int id) => Id = id;

    // Properties

    // Properties: Status & meta-data

    public ComplaintStatus Status { get; set; } = ComplaintStatus.New;

    public DateTimeOffset EnteredDate { get; init; } = DateTimeOffset.Now;

    public ApplicationUser EnteredBy { get; set; } = default!;

    public DateTimeOffset ReceivedDate { get; set; }

    public ApplicationUser ReceivedBy { get; set; } = default!;

    // Properties: Caller

    public string? CallerName { get; set; }
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

    public string? ComplaintNature { get; set; }

    public string? ComplaintLocation { get; set; }

    public string? ComplaintDirections { get; set; }

    [StringLength(50)]
    public string? ComplaintCity { get; set; }

    [StringLength(15)]
    public string? ComplaintCounty { get; set; }

    public Concern PrimaryConcern { get; set; } = default!;

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

    public Office CurrentOffice { get; set; } = default!;

    [StringLength(50)]
    [Obsolete("Holdover from old application")]
    public string? CurrentProgram { get; set; }

    public ApplicationUser? CurrentOwner { get; set; }

    public DateTimeOffset? CurrentOwnerAssignedDate { get; set; }

    public Guid? CurrentAssignmentTransitionId { get; set; }

    public DateTimeOffset? CurrentOwnerAcceptedDate { get; set; }

    public List<ComplaintTransition> ComplaintTransitions { get; set; } = new();

    // Properties: Actions

    public List<ComplaintAction> ComplaintActions { get; set; } = new();

    // Properties: Review/Closure

    public ApplicationUser? ReviewedBy { get; set; }

    public string? ReviewComments { get; set; }

    public bool ComplaintClosed { get; set; }

    public DateTimeOffset? ComplaintClosedDate { get; set; }

    // Properties: Deletion

    public ApplicationUser? DeletedBy { get; set; }
    public string? DeleteComments { get; set; }

    // Properties: Attachments

    public List<Attachment> Attachments { get; set; } = new();

    // Methods
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
    ///  Represents a complaint that has been approved by a reviewer.
    /// </summary>
    [Display(Name = "Approved/Closed")] Closed = 3,

    /// <summary>
    ///  Represents a complaint that has been administratively closed (e.g., by EPD-IT).
    /// </summary>
    [Display(Name = "Administratively Closed")]
    AdministrativelyClosed = 4,
}
