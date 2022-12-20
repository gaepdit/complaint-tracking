using Cts.AppServices.Offices;
using Cts.Domain.Complaints;
using Cts.Domain.Concerns;
using Cts.Domain.DataProcessing;
using Cts.Domain.ValueObjects;
using System.ComponentModel;

namespace Cts.AppServices.Complaints;

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

    [DisplayName("Date Received")]
    public DateTime DateReceived { get; init; }

    // Properties: Complaint details

    [DisplayName("Nature of Complaint")]
    public string? ComplaintNature
    {
        get => _complaintNature;
        init => _complaintNature = PersonalInformation.RedactPii(value);
    }

    [DisplayName("Location of Complaint")]
    public string? ComplaintLocation
    {
        get => _complaintLocation;
        init => _complaintLocation = PersonalInformation.RedactPii(value);
    }
    
    [DisplayName("City of Complaint")]
    public string? ComplaintCity { get; init; }

    [DisplayName("County of Complaint")]
    public string? ComplaintCounty { get; init; }

    [DisplayName("Primary Concern")]
    public Concern? PrimaryConcern { get; init; }

    public string? PrimaryConcernName => PrimaryConcern?.Name;
    
    [DisplayName("Secondary Concern")]
    public Concern? SecondaryConcern { get; init; }
    public string? SecondaryConcernName => SecondaryConcern?.Name;

    // Properties: Source

    [DisplayName("Facility ID Number")]
    public string? SourceFacilityId { get; set; }

    [DisplayName("Source Name")]
    public string? SourceFacilityName { get; set; }

    [DisplayName("Source Contact")]
    public string? SourceContactName { get; set; }

    [DisplayName("Source Address")]
    public Address? SourceAddress { get; set; }
    
    // Properties: Assignment/History

    [DisplayName("Assigned Office")]
    public OfficeViewDto? CurrentOffice { get; init; }

    // Properties: Actions
    // TODO
    // [DisplayName("Actions")]
    // public List<ComplaintActionPublicViewDto> ComplaintActions { get; set; } = new();

    // Properties: Review/Closure

    [DisplayName("Date Complaint Closed")]
    public DateTime? DateComplaintClosed { get; init; }

    [DisplayName("Review Comments")]
    public string? ReviewComments
    {
        get => _reviewComments;
        init => _reviewComments = PersonalInformation.RedactPii(value);
    }

    // Properties: Attachments
    // TODO
    // [DisplayName("Attachments")]
    // public List<AttachmentPublicViewDto> Attachments { get; set; } = new();


}
