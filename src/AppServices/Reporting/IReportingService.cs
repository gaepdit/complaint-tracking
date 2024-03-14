using Cts.Domain.DataViews.ReportingViews;

namespace Cts.AppServices.Reporting;

public interface IReportingService : IDisposable, IAsyncDisposable
{
    Task<List<StaffReportView>> ComplaintsAssignedToInactiveUsersAsync();
    Task<List<StaffReportView>> ComplaintsByStaffAsync(Guid officeId, DateOnly dateFrom, DateOnly dateTo);
    Task<List<StaffReportView>> DaysSinceMostRecentActionAsync(Guid officeId, int threshold);

    Task<List<OfficeReportView>> DaysToClosureByOfficeAsync(DateOnly dateFrom, DateOnly dateTo,
        bool includeAdminClosed);

    Task<List<StaffReportView>> DaysToClosureByStaffAsync(Guid officeId, DateOnly dateFrom, DateOnly dateTo,
        bool includeAdminClosed);

    Task<List<StaffReportView>> DaysToFollowupByStaffAsync(Guid office, DateOnly dateFrom, DateOnly dateTo);
}
