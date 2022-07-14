namespace Cts.Domain.Entities;

public class Complaint : IAuditable
{
    public int Id { get; set; }

    // Meta-data

    public ComplaintStatus Status { get; set; } = ComplaintStatus.New;

    public DateTime DateEntered { get; set; }

    public ApplicationUser EnteredBy { get; set; } = null!;
    public string EnteredById { get; set; } = "";

    public DateTime DateReceived { get; set; }

    public ApplicationUser ReceivedBy { get; set; } = null!;
    public string ReceivedById { get; set; } = "";

    // Caller

    public string? CallerName { get; set; }

    public string? CallerRepresents { get; set; }

    [StringLength(100)]
    public string? CallerStreet { get; set; }

    [StringLength(100)]
    public string? CallerStreet2 { get; set; }

    [StringLength(50)]
    public string? CallerCity { get; set; }

    public string? CallerState { get; set; }

    [StringLength(10)]
    [DataType(DataType.PostalCode)]
    public string? CallerPostalCode { get; set; }

    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    public string? CallerPhoneNumber { get; set; }

    public PhoneType? CallerPhoneType { get; set; }

    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    public string? CallerSecondaryPhoneNumber { get; set; }

    public PhoneType? CallerSecondaryPhoneType { get; set; }

    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    public string? CallerTertiaryPhoneNumber { get; set; }

    public PhoneType? CallerTertiaryPhoneType { get; set; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string? CallerEmail { get; set; }

    // Complaint

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

    public Concern? PrimaryConcern { get; set; }
    public Guid? PrimaryConcernId { get; set; }

    public Concern? SecondaryConcern { get; set; }
    public Guid? SecondaryConcernId { get; set; }

    // Source

    [StringLength(50)]
    public string? SourceFacilityId { get; set; }

    [StringLength(100)]
    public string? SourceFacilityName { get; set; }

    [StringLength(100)]
    public string? SourceContactName { get; set; }

    [StringLength(100)]
    public string? SourceStreet { get; set; }

    [StringLength(100)]
    public string? SourceStreet2 { get; set; }

    [StringLength(50)]
    public string? SourceCity { get; set; }

    public string? SourceState { get; set; }

    [StringLength(10)]
    [DataType(DataType.PostalCode)]
    public string? SourcePostalCode { get; set; }

    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    public string? SourcePhoneNumber { get; set; }

    public PhoneType? SourcePhoneType { get; set; }

    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    public string? SourceSecondaryPhoneNumber { get; set; }

    public PhoneType? SourceSecondaryPhoneType { get; set; }

    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    public string? SourceTertiaryPhoneNumber { get; set; }

    public PhoneType? SourceTertiaryPhoneType { get; set; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string? SourceEmail { get; set; }

    // Assignment/History

    public virtual Office CurrentOffice { get; set; } = null!;
    public Guid CurrentOfficeId { get; set; }

    [StringLength(50)]
    [Obsolete("Holdover from old application")]
    public string? CurrentProgram { get; set; }

    public virtual ApplicationUser? CurrentOwner { get; set; }
    public string? CurrentOwnerId { get; set; }

    public DateTime? DateCurrentOwnerAssigned { get; set; }

    public Guid? CurrentAssignmentTransitionId { get; set; }

    public DateTime? DateCurrentOwnerAccepted { get; set; }

    public List<ComplaintTransition> ComplaintTransitions { get; set; } = new();

    // Actions

    public List<ComplaintAction> ComplaintActions { get; set; } = new();

    // Review/Closure

    public ApplicationUser? ReviewBy { get; set; }
    public string? ReviewById { get; set; }

    [StringLength(4000)]
    public string? ReviewComments { get; set; }

    public bool ComplaintClosed { get; set; }

    public DateTime? DateComplaintClosed { get; set; }

    // Deletion

    public bool Deleted { get; set; }

    public ApplicationUser? DeletedBy { get; set; }
    public string? DeletedById { get; set; }

    public DateTime? DateDeleted { get; set; }

    [StringLength(4000)]
    public string? DeleteComments { get; set; }

    // Attachments

    public List<Attachment> Attachments { get; set; } = new();
}

// enums

public enum PhoneType
{
    Cell = 0,
    Fax = 1,
    Home = 2,
    Office = 3,
}

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
