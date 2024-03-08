using Cts.Domain.Entities.Complaints;

namespace Cts.Domain.DataViews.ReportingViews;

public class ComplaintView
{
    [Display(Name = "Complaint ID")]
    public int Id { get; init; }

    [Display(Name = "County of Complaint")]
    public string? ComplaintCounty { get; init; }

    [Display(Name = "Source Name")]
    public string? SourceFacilityName { get; init; }

    [Display(Name = "Date Received")]
    public DateTimeOffset ReceivedDate { get; init; }

    [Display(Name = "Date Closed")]
    public DateTimeOffset? ComplaintClosedDate { get; init; }

    public ComplaintStatus Status { get; init; }

    [Display(Name = "Most Recent Action Date")]
    public DateTimeOffset? LastActionDate { get; init; }

    [Display(Name = "Days Since Last Action")]
    public int? DaysSinceLastAction { get; init; }

    // Calculated properties

    [Display(Name = "Days to Closure")]
    public int? DaysToClosure => ComplaintClosedDate?.Date.Subtract(ReceivedDate.Date).Days;
}
