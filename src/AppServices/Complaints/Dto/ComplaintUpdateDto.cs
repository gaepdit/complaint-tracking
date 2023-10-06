using Cts.Domain.Entities.Complaints;
using Cts.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.Dto;

public class ComplaintUpdateDto : IComplaintDtoDetails
{
    // Authorization handler assist properties
    public bool ComplaintClosed { get; init; }
    public bool IsDeleted { get; init; }
    public string? CurrentOwnerId { get; init; }
    public Guid? CurrentOfficeId { get; init; }
    public Guid CurrentUserOfficeId { get; set; }
    public string? EnteredById { get; init; }
    public DateTimeOffset EnteredDate { get; init; }
    public DateTimeOffset? CurrentOwnerAcceptedDate { get; init; }
    public ComplaintStatus Status { get; init; }

    // Meta-data

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date received")]
    public DateOnly ReceivedDate { get; init; }

    [Required]
    [DataType(DataType.Time)]
    [Display(Name = "Time received")]
    public TimeOnly ReceivedTime { get; init; }

    [Required]
    [Display(Name = "Received by")]
    public string? ReceivedById { get; init; }

    // Caller

    [Display(Name = "Name")]
    public string? CallerName { get; init; }

    [Display(Name = "Represents")]
    public string? CallerRepresents { get; init; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string? CallerEmail { get; init; }

    [Display(Name = "Primary phone")]
    public PhoneNumber? CallerPhoneNumber { get; init; }

    [Display(Name = "Secondary phone")]
    public PhoneNumber? CallerSecondaryPhoneNumber { get; init; }

    [Display(Name = "Other phone")]
    public PhoneNumber? CallerTertiaryPhoneNumber { get; init; }

    [Display(Name = "Caller address")]
    public IncompleteAddress CallerAddress { get; init; } = new();

    // Complaint details

    [Required]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Nature of Complaint")]
    public string? ComplaintNature { get; init; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Location of Complaint")]
    public string? ComplaintLocation { get; init; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Directions to Complaint")]
    public string? ComplaintDirections { get; init; }

    [StringLength(50)]
    [Display(Name = "City of Complaint")]
    public string? ComplaintCity { get; init; }

    [StringLength(15)]
    [Display(Name = "County of Complaint")]
    public string? ComplaintCounty { get; init; }

    [Required]
    [Display(Name = "Primary concern")]
    public Guid PrimaryConcernId { get; init; }

    [Display(Name = "Secondary concern")]
    public Guid? SecondaryConcernId { get; init; }

    // Source

    [StringLength(100)]
    [Display(Name = "Source/facility name")]
    public string? SourceFacilityName { get; init; }

    [StringLength(50)]
    [Display(Name = "Facility ID Number")]
    public string? SourceFacilityIdNumber { get; init; }

    // Source Contact

    [StringLength(100)]
    [Display(Name = "Name")]
    public string? SourceContactName { get; init; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string? SourceEmail { get; init; }

    [Display(Name = "Primary phone")]
    public PhoneNumber? SourcePhoneNumber { get; init; }

    [Display(Name = "Secondary phone")]
    public PhoneNumber? SourceSecondaryPhoneNumber { get; init; }

    [Display(Name = "Other phone")]
    public PhoneNumber? SourceTertiaryPhoneNumber { get; init; }

    // Source Address

    [Display(Name = "Source address")]
    public IncompleteAddress SourceAddress { get; init; } = new();
}
