using Cts.Domain.Entities.Complaints;

namespace Cts.Domain.DataViews.ReportingViews;

public class ComplaintView
{
    public int Id { get; init; }
    public string? ComplaintCounty { get; init; }
    public string? SourceFacilityName { get; init; }
    public DateTimeOffset ReceivedDate { get; init; }
    public DateTimeOffset? ComplaintClosedDate { get; init; }
    public ComplaintStatus Status { get; init; }
    
    public DateTimeOffset? MostRecentActionDate { get; init; }
    public int? DaysSinceMostRecentAction { get; init; }

    // Calculated properties
    public int? DaysToClosure => ComplaintClosedDate?.Date.Subtract(ReceivedDate.Date).Days;
}
