using Cts.Domain.DataViews.DataArchiveViews;
using Cts.Domain.DataViews.ReportingViews;

namespace Cts.Domain.DataViews;

public interface IDataViewRepository : IDisposable, IAsyncDisposable
{
    // Data archive export
    Task<List<OpenComplaint>> OpenComplaintsAsync(CancellationToken token);
    Task<List<ClosedComplaint>> ClosedComplaintsAsync(CancellationToken token);
    Task<List<ClosedComplaintAction>> ClosedComplaintActionsAsync(CancellationToken token);
    Task<List<RecordsCount>> RecordsCountAsync(CancellationToken token);

    // Reporting
    Task<List<StaffReportView>> ComplaintsAssignedToInactiveUsersAsync();
    Task<List<StaffReportView>> ComplaintsByStaffAsync(Guid officeId, DateOnly dateFrom, DateOnly dateTo);
    Task<List<StaffReportView>> DaysSinceMostRecentActionAsync(Guid officeId, int threshold);

    Task<List<OfficeReportView>> DaysToClosureByOfficeAsync(DateOnly dateFrom, DateOnly dateTo,
        bool includeAdminClosed);

    Task<List<StaffReportView>> DaysToClosureByStaffAsync(Guid officeId, DateOnly dateFrom, DateOnly dateTo,
        bool includeAdminClosed);

    Task<List<StaffReportView>> DaysToFollowupByStaffAsync(Guid officeId, DateOnly dateFrom, DateOnly dateTo);
}
