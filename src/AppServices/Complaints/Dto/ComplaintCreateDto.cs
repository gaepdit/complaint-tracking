using Cts.AppServices.Utilities;
using Cts.Domain.ValueObjects;
using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.Dto;

public class ComplaintCreateDto
{
    // Constructors

    [UsedImplicitly]
    public ComplaintCreateDto() { }

    public ComplaintCreateDto(string? receivedById)
    {
        ReceivedById = receivedById;
    }

    // Meta-data

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date received")]
    public DateTime ReceivedDate { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.Time)]
    [Display(Name = "Time received")]
    public DateTime TimeReceived { get; set; } = DateTime.Now.RoundToNearestQuarterHour();

    [Required]
    [Display(Name = "Received by")]
    public string? ReceivedById { get; set; }

    // Caller

    [Display(Name = "Represents")]
    public string? CallerRepresents { get; set; }

    [Display(Name = "Name")]
    public string? CallerName { get; set; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string? CallerEmail { get; set; }

    [Display(Name = "Primary phone")]
    public PhoneNumber? CallerPhoneNumber { get; set; }

    [Display(Name = "Secondary phone")]
    public PhoneNumber? CallerSecondaryPhoneNumber { get; set; }

    [Display(Name = "Other phone")]
    public PhoneNumber? CallerTertiaryPhoneNumber { get; set; }

    [Display(Name = "Caller address")]
    public IncompleteAddress CallerAddress { get; set; } = new();

    // Complaint details

    [Required]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Nature of Complaint")]
    public string? ComplaintNature { get; set; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Location of Complaint")]
    public string? ComplaintLocation { get; set; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Direction to Complaint")]
    public string? ComplaintDirections { get; set; }

    [StringLength(50)]
    [Display(Name = "City of Complaint")]
    public string? ComplaintCity { get; set; }

    [StringLength(15)]
    [Display(Name = "County of Complaint")]
    public string? ComplaintCounty { get; set; }

    [Required]
    [Display(Name = "Primary concern")]
    public Guid PrimaryConcernId { get; set; }

    [Display(Name = "Secondary concern")]
    public Guid? SecondaryConcernId { get; set; }

    // Source

    [StringLength(100)]
    [Display(Name = "Source/facility name")]
    public string? SourceFacilityName { get; set; }

    [StringLength(50)]
    [Display(Name = "Facility ID Number")]
    public string? SourceFacilityIdNumber { get; set; }

    [StringLength(100)]
    [Display(Name = "Name")]
    public string? SourceContactName { get; set; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string? SourceEmail { get; set; }

    [Display(Name = "Primary phone")]
    public PhoneNumber? SourcePhoneNumber { get; set; } = default!;

    [Display(Name = "Secondary phone")]
    public PhoneNumber? SourceSecondaryPhoneNumber { get; set; } = default!;

    [Display(Name = "Other phone")]
    public PhoneNumber? SourceTertiaryPhoneNumber { get; set; } = default!;

    [Display(Name = "Source address")]
    public IncompleteAddress SourceAddress { get; set; } = new();

    // Assignment

    [Required]
    [Display(Name = "Assigned office")]
    public Guid CurrentOfficeId { get; set; }

    [Display(Name = "Assigned associate")]
    public string? CurrentOwnerId { get; set; }
}
