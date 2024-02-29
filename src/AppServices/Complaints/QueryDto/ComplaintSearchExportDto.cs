﻿using ClosedXML.Attributes;
using Cts.Domain.Entities.Complaints;
using GaEpd.AppLibrary.Extensions;

namespace Cts.AppServices.Complaints.QueryDto;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record ComplaintSearchExportDto
{
    public ComplaintSearchExportDto(Complaint complaint)
    {
        ComplaintId = complaint.Id;
        ReceivedByName = complaint.ReceivedBy.SortableFullName;
        DateReceived = complaint.ReceivedDate;
        Status = complaint.Status.GetDisplayName();
        DateComplaintClosed = complaint.ComplaintClosedDate;
        ReviewComments = complaint.ReviewComments;
        SourceFacilityName = complaint.SourceFacilityName;
        SourceLocation = complaint.SourceAddress?.CityState;
        SourceFacilityId = complaint.SourceFacilityIdNumber;
        CurrentOwnerName = complaint.CurrentOwner?.SortableFullName;
        CurrentOfficeName = complaint.CurrentOffice.Name;
        PrimaryConcern = complaint.PrimaryConcern.Name;
        SecondaryConcern = complaint.SecondaryConcern?.Name;
        ComplaintNature = complaint.ComplaintNature;
        Deleted = complaint.IsDeleted ? "Deleted" : "No";

        var action = complaint.ComplaintActions.SingleOrDefault();
        ActionDate = action?.ActionDate;
        ActionType = action?.ActionType.Name;
        ActionComments = action?.Comments;
    }

    [XLColumn(Header = "Complaint ID")]
    public int ComplaintId { get; init; }

    [XLColumn(Header = "Received By")]
    public string ReceivedByName { get; init; }

    [XLColumn(Header = "Date Received")]
    public DateTimeOffset DateReceived { get; init; }

    [XLColumn(Header = "Status")]
    public string Status { get; init; }

    [XLColumn(Header = "Date Complaint Closed")]
    public DateTimeOffset? DateComplaintClosed { get; init; }

    [XLColumn(Header = "Review Comments")]
    public string? ReviewComments { get; init; }

    [XLColumn(Header = "Source Name")]
    public string? SourceFacilityName { get; init; }

    [XLColumn(Header = "Source Location")]
    public string? SourceLocation { get; init; }

    [XLColumn(Header = "Facility ID")]
    public string? SourceFacilityId { get; init; }

    [XLColumn(Header = "Current Assignment")]
    public string? CurrentOwnerName { get; init; }

    [XLColumn(Header = "EPD Office")]
    public string CurrentOfficeName { get; init; }

    [XLColumn(Header = "Primary Area of Concern")]
    public string PrimaryConcern { get; init; }

    [XLColumn(Header = "Secondary Area of Concern")]
    public string? SecondaryConcern { get; init; }

    [XLColumn(Header = "Nature of Complaint")]
    public string? ComplaintNature { get; init; }

    [XLColumn(Header = "Most Recent Action")]
    public DateOnly? ActionDate { get; init; }

    [XLColumn(Header = "Action Type")]
    public string? ActionType { get; init; }

    [XLColumn(Header = "Action Comments")]
    public string? ActionComments { get; init; }

    [XLColumn(Header = "Deleted?")]
    public string Deleted { get; init; }
}
