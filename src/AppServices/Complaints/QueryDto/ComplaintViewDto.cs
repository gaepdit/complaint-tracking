using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Complaints.QueryDto;

public record ComplaintViewDto
{
    public int Id { get; init; }

    // Properties: Status & meta-data

    public ComplaintStatus Status { get; init; }

    [Display(Name = "Date Received")]
    public DateTimeOffset ReceivedDate { get; init; }

    [Display(Name = "Received By")]
    public StaffViewDto? ReceivedBy { get; init; }

    [Display(Name = "Entered By")]
    public StaffViewDto? EnteredBy { get; init; }

    [Display(Name = "Date Entered")]
    public DateTimeOffset EnteredDate { get; init; }

    // Properties: Complaint details

    [Display(Name = "Nature of Complaint")]
    public string? ComplaintNature { get; init; }

    [Display(Name = "Location of Complaint")]
    public string? ComplaintLocation { get; init; }

    [Display(Name = "Directions to Complaint")]
    public string? ComplaintDirections { get; init; }

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

    [Display(Name = "Primary Phone")]
    public PhoneNumber? SourcePhoneNumber { get; init; }

    [Display(Name = "Secondary Phone")]
    public PhoneNumber? SourceSecondaryPhoneNumber { get; init; }

    [Display(Name = "Other Phone")]
    public PhoneNumber? SourceTertiaryPhoneNumber { get; init; }

    [Display(Name = "Email Address")]
    public string? SourceEmail { get; init; }

    // Properties: Assignment

    [Display(Name = "Current Assigned Office")]
    public OfficeWithAssignorDto? CurrentOffice { get; init; }

    public string? CurrentOfficeAssignorId { get; init; }

    [Display(Name = "Current Assigned Office")]
    public string? CurrentOfficeName { get; init; }

    [Display(Name = "Current Assigned Staff")]
    public StaffViewDto? CurrentOwner { get; init; }

    [Display(Name = "Date Assigned")]
    public DateTimeOffset? CurrentOwnerAssignedDate { get; init; }

    [Display(Name = "Date Accepted")]
    public DateTimeOffset? CurrentOwnerAcceptedDate { get; init; }

    // Properties: Caller

    [Display(Name = "Name")]
    public string? CallerName { get; init; }

    [Display(Name = "Represents")]
    public string? CallerRepresents { get; init; }

    [Display(Name = "Caller Address")]
    public IncompleteAddress? CallerAddress { get; init; }

    [Display(Name = "Primary Phone")]
    public PhoneNumber? CallerPhoneNumber { get; init; }

    [Display(Name = "Secondary Phone")]
    public PhoneNumber? CallerSecondaryPhoneNumber { get; init; }

    [Display(Name = "Other Phone")]
    public PhoneNumber? CallerTertiaryPhoneNumber { get; init; }

    [Display(Name = "Email Address")]
    public string? CallerEmail { get; init; }

    // Properties: Review/Closure

    [Display(Name = "Complaint Closed")]
    public bool ComplaintClosed { get; init; }

    [Display(Name = "Date Closed")]
    public DateTimeOffset? ComplaintClosedDate { get; init; }

    [Display(Name = "Reviewed By")]
    public StaffViewDto? ReviewedBy { get; init; }

    [Display(Name = "Review Comments")]
    public string? ReviewComments { get; init; }

    // Properties: Deletion

    [Display(Name = "Deleted?")]
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Comments")]
    public string? DeleteComments { get; init; }

    // === Lists ===

    [UsedImplicitly]
    public List<ActionViewDto> Actions { get; } = [];

    [UsedImplicitly]
    public List<AttachmentViewDto> Attachments { get; } = [];

    [UsedImplicitly]
    public List<ComplaintTransitionViewDto> ComplaintTransitions { get; } = [];

    public DateTimeOffset? EarliestTransition =>
        ComplaintTransitions.Count > 0 ? ComplaintTransitions[0].CommittedDate : null;

    // === Calculated properties ===

    public bool IsPublic => ComplaintClosed && !IsDeleted;
}
