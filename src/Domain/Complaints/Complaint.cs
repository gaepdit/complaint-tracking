using Cts.Domain.AppLibraryExtra;
using Cts.Domain.Attachments;
using Cts.Domain.ComplaintActions;
using Cts.Domain.ComplaintTransitions;
using Cts.Domain.Concerns;
using Cts.Domain.Identity;
using Cts.Domain.Offices;

namespace Cts.Domain.Complaints;

public class Complaint : AuditableSoftDeleteEntity<int>, ISoftDelete
{
    // Constants


    // Constructors

    internal Complaint() { }

    internal Complaint(int id) : base(id) { }

    // Properties

    // Properties: Status & meta-data

    public ComplaintStatus Status { get; set; } = ComplaintStatus.New;

    public DateTime DateEntered { get; init; } = DateTime.Now;

    public ApplicationUser EnteredBy { get; init; } = null!;

    public DateTime DateReceived { get; set; }

    public ApplicationUser ReceivedBy { get; set; } = null!;

    // Properties: Caller

    public string? CallerName { get; set; }
    public string? CallerRepresents { get; set; }
    public Address? CallerAddress { get; set; }

    public PhoneNumber? CallerPhoneNumber { get; set; }
    public PhoneNumber? CallerSecondaryPhoneNumber { get; set; }
    public PhoneNumber? CallerTertiaryPhoneNumber { get; set; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string? CallerEmail { get; set; }

    // Properties: Complaint details

    [DataType(DataType.MultilineText)]
    public string? ComplaintNature { get; set; }

    [DataType(DataType.MultilineText)]
    [StringLength(4000)]
    public string? ComplaintLocation { get; set; }

    [DataType(DataType.MultilineText)]
    [StringLength(4000)]
    public string? ComplaintDirections { get; set; }

    [StringLength(50)]
    public string? ComplaintCity { get; set; }

    [StringLength(15)]
    public string? ComplaintCounty { get; set; }

    public Concern PrimaryConcern { get; set; } = default!;

    public Concern? SecondaryConcern { get; set; }

    // Properties: Source

    [StringLength(50)]
    public string? SourceFacilityId { get; set; }

    [StringLength(100)]
    public string? SourceFacilityName { get; set; }

    [StringLength(100)]
    public string? SourceContactName { get; set; }

    public Address? SourceAddress { get; set; }

    public PhoneNumber? SourcePhoneNumber { get; set; }
    public PhoneNumber? SourceSecondaryPhoneNumber { get; set; }
    public PhoneNumber? SourceTertiaryPhoneNumber { get; set; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string? SourceEmail { get; set; }

    // Properties: Assignment/History

    public Office CurrentOffice { get; set; } = null!;

    [StringLength(50)]
    [Obsolete("Holdover from old application")]
    public string? CurrentProgram { get; set; }

    public ApplicationUser? CurrentOwner { get; set; }

    public DateTime? DateCurrentOwnerAssigned { get; set; }

    public Guid? CurrentAssignmentTransitionId { get; set; }

    public DateTime? DateCurrentOwnerAccepted { get; set; }

    public List<ComplaintTransition> ComplaintTransitions { get; set; } = new();

    // Properties: Actions

    public List<ComplaintAction> ComplaintActions { get; set; } = new();

    // Properties: Review/Closure

    public ApplicationUser? ReviewBy { get; set; }

    [StringLength(4000)]
    public string? ReviewComments { get; set; }

    public bool ComplaintClosed { get; set; }

    public DateTime? DateComplaintClosed { get; set; }

    // Properties: Deletion

    [StringLength(4000)]
    public string? DeleteComments { get; set; }

    // Properties: Attachments

    public List<Attachment> Attachments { get; set; } = new();

    // Methods
}

// Enums

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
