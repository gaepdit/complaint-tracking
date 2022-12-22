using Cts.AppServices.Offices;
using Cts.Domain.Complaints;
using Cts.Domain.Concerns;
using Cts.Domain.DataProcessing;
using Cts.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.Dto;

public class ComplaintPublicViewDto
{
    // Backing fields
    private readonly string? _complaintNature;
    private readonly string? _complaintLocation;
    private readonly string? _reviewComments;

    // Properties

    public int Id { get; init; }

    // Properties: Status & meta-data

    public ComplaintStatus Status { get; init; } = ComplaintStatus.New;

    [Display(Name = "Date Received")]
    public DateTime DateReceived { get; init; }

    // Properties: Complaint details

    [Display(Name = "Nature of Complaint")]
    public string? ComplaintNature
    {
        get => _complaintNature;
        init => _complaintNature = PersonalInformation.RedactPii(value);
    }

    [Display(Name = "Location of Complaint")]
    public string? ComplaintLocation
    {
        get => _complaintLocation;
        init => _complaintLocation = PersonalInformation.RedactPii(value);
    }

    [Display(Name = "City of Complaint")]
    public string? ComplaintCity { get; init; }

    [Display(Name = "County of Complaint")]
    public string? ComplaintCounty { get; init; }

    [Display(Name = "Primary Concern")]
    public Concern? PrimaryConcern { get; init; }

    public string? PrimaryConcernName => PrimaryConcern?.Name;

    [Display(Name = "Secondary Concern")]
    public Concern? SecondaryConcern { get; init; }

    public string? SecondaryConcernName => SecondaryConcern?.Name;

    // Properties: Source

    [Display(Name = "Facility ID Number")]
    public string? SourceFacilityId { get; set; }

    [Display(Name = "Source Name")]
    public string? SourceFacilityName { get; set; }

    [Display(Name = "Source Contact")]
    public string? SourceContactName { get; set; }

    [Display(Name = "Source Address")]
    public Address? SourceAddress { get; set; }

    // Properties: Assignment/History

    [Display(Name = "Assigned Office")]
    public OfficeViewDto? CurrentOffice { get; init; }

    public string? CurrentOfficeName => CurrentOffice?.Name;

    // Properties: Actions
    // TODO
    // [Display(Name = "Actions")]
    // public List<ComplaintActionPublicViewDto> ComplaintActions { get; set; } = new();

    // Properties: Review/Closure

    [Display(Name = "Date Complaint Closed")]
    public DateTime? DateComplaintClosed { get; init; }

    [Display(Name = "Review Comments")]
    public string? ReviewComments
    {
        get => _reviewComments;
        init => _reviewComments = PersonalInformation.RedactPii(value);
    }

    // Properties: Attachments
    // TODO
    // [Display(Name = "Attachments")]
    // public List<AttachmentPublicViewDto> Attachments { get; set; } = new();
}
