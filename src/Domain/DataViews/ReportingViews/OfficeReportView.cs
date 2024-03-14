namespace Cts.Domain.DataViews.ReportingViews;

public class OfficeReportView
{
    public Guid OfficeId { get; init; }
    public string? OfficeName { get; init; }
    public int? TotalComplaintsCount { get; init; }
    public int? TotalDaysToClosure { get; init; }
    public double? AverageDaysToClosure { get; init; }
}
