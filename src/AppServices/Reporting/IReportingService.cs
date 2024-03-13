using Cts.Domain.DataViews.ReportingViews;

namespace Cts.AppServices.Reporting;

public interface IReportingService : IDisposable, IAsyncDisposable
{
    Task<List<StaffViewWithComplaints>> DaysSinceMostRecentActionAsync(Guid officeId, int threshold);

    Task<List<ComplaintView>> ComplaintsAssignedToInactiveUsersAsync(Guid officeId);

    Task<List<StaffViewWithComplaints>> DaysToClosureByStaffAsync(Guid officeId, DateOnly dateFrom, DateOnly dateTo,
        bool includeAdminClosed);

    Task<List<StaffViewWithComplaints>> DaysToFollowupByStaffAsync(Guid office, DateOnly dateFrom, DateOnly dateTo);
}
