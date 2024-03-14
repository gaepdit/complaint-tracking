using GaEpd.AppLibrary.Extensions;

namespace Cts.Domain.DataViews.ReportingViews;

public class StaffReportView
{
    public string Id { get; init; } = string.Empty;
    public Guid OfficeId { get; init; }
    public string GivenName { get; init; } = string.Empty;
    public string FamilyName { get; init; } = string.Empty;
    public List<ComplaintReportView> Complaints { get; init; } = [];

    // Calculated properties
    public string SortableFullName => new[] { FamilyName, GivenName }.ConcatWithSeparator(", ");
    public double? AverageDaysToClosure => Complaints.Average(complaint => complaint.DaysToClosure);
    public double? AverageDaysToFollowup => Complaints.Average(complaint => complaint.DaysToFollowup);
}
