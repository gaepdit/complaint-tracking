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
    Task<List<StaffViewWithComplaints>> DaysSinceMostRecentActionAsync(Guid officeId, int threshold);

    Task<List<ComplaintView>> ComplaintsAssignedToInactiveUsersAsync(Guid officeId);

    Task<List<StaffViewWithComplaints>> DaysToClosureByStaffAsync(Guid officeId, DateOnly dateFrom, DateOnly dateTo,
        bool includeAdminClosed);
}
