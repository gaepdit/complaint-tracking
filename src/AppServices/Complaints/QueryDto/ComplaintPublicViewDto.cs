using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.ComplaintActions.Dto;
using Cts.Domain.DataProcessing;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.QueryDto;

public record ComplaintPublicViewDto
{
    // Backing fields
    private readonly string? _complaintNature;
    private readonly string? _complaintLocation;
    private readonly string? _reviewComments;

    // Properties

    public int Id { get; init; }

    // Properties: Status & meta-data

    public ComplaintStatus Status { get; init; }
    public bool ComplaintClosed { get; init; }


    [Display(Name = "Complaint Received")]
    public DateTimeOffset ReceivedDate { get; init; }

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
    public string? PrimaryConcernName { get; init; }

    [Display(Name = "Secondary Concern")]
    public string? SecondaryConcernName { get; init; }

    // Properties: Source

    [Display(Name = "Facility ID Number")]
    public string? SourceFacilityIdNumber { get; init; }

    [Display(Name = "Source Name")]
    public string? SourceFacilityName { get; init; }

    [Display(Name = "Contact Name")]
    public string? SourceContactName { get; init; }

    [Display(Name = "Source Address")]
    public IncompleteAddress? SourceAddress { get; init; }

    // Properties: Assignment/History

    [Display(Name = "Assigned Office")]
    public string? CurrentOfficeName { get; init; }

    // Properties: Review/Closure

    [Display(Name = "Complaint Closed")]
    public DateTimeOffset? ComplaintClosedDate { get; init; }

    [Display(Name = "Review Comments")]
    public string? ReviewComments
    {
        get => _reviewComments;
        init => _reviewComments = PersonalInformation.RedactPii(value);
    }

    // === Lists ===

    // Properties: Actions

    [UsedImplicitly]
    public List<ActionPublicViewDto> Actions { get; } = [];

    // Properties: Attachments

    [UsedImplicitly]
    public List<AttachmentViewDto> Attachments { get; } = [];
}
