using Cts.AppServices.Utilities;
using Cts.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.Dto;

public class ComplaintCreateDto
{
    // Constructors

    [UsedImplicitly]
    public ComplaintCreateDto() { }

    public ComplaintCreateDto(string? receivedById, Guid? currentOfficeId)
    {
        ReceivedById = receivedById;
        CurrentOfficeId = currentOfficeId;
    }

    // Meta-data

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date received")]
    public DateTime ReceivedDate { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.Time)]
    [Display(Name = "Time received")]
    public DateTime ReceivedTime { get; init; } = DateTime.Now.RoundToNearestQuarterHour();

    [Required]
    [Display(Name = "Received by")]
    public string? ReceivedById { get; init; }

    // Caller

    [Display(Name = "Represents")]
    public string? CallerRepresents { get; init; }

    [Display(Name = "Name")]
    public string? CallerName { get; init; }

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
    [Display(Name = "Direction to Complaint")]
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

    [Display(Name = "Source address")]
    public IncompleteAddress SourceAddress { get; init; } = new();

    // Assignment

    [Required]
    [Display(Name = "Assigned office")]
    public Guid? CurrentOfficeId { get; init; }

    [Display(Name = "Assigned associate")]
    public string? CurrentOwnerId { get; init; }
}
