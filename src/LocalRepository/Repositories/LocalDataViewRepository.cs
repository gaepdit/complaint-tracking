using Cts.Domain.DataViews;
using Cts.Domain.DataViews.DataArchiveViews;
using Cts.Domain.DataViews.ReportingViews;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalDataViewRepository : IDataViewRepository
{
    // Data archive export
    public Task<List<OpenComplaint>> OpenComplaintsAsync(CancellationToken token) =>
        throw new NotImplementedException();

    public Task<List<ClosedComplaint>> ClosedComplaintsAsync(CancellationToken token) =>
        throw new NotImplementedException();

    public Task<List<ClosedComplaintAction>> ClosedComplaintActionsAsync(CancellationToken token) =>
        throw new NotImplementedException();

    public Task<List<RecordsCount>> RecordsCountAsync(CancellationToken token) =>
        Task.FromResult<List<RecordsCount>>([
            new RecordsCount("Open Complaints", 20, 1),
            new RecordsCount("Closed Complaints", 9999, 2),
            new RecordsCount("Complaint Actions", 12345, 3),
        ]);

    // Reporting
    public Task<List<StaffViewWithComplaints>> DaysSinceMostRecentActionAsync(Guid officeId, int threshold) =>
        Task.FromResult(DataViewsTestData.GetStaffViewWithComplaintsData());

    public Task<List<ComplaintView>> ComplaintsAssignedToInactiveUsersAsync(Guid officeId) => 
        throw new NotImplementedException();

    public Task<List<StaffViewWithComplaints>> DaysToClosureByStaffAsync(Guid officeId, DateOnly dateFrom, DateOnly dateTo, bool includeAdminClosed) => 
        throw new NotImplementedException();

    void IDisposable.Dispose()
    {
        // Method intentionally left empty.
    }

    ValueTask IAsyncDisposable.DisposeAsync() => ValueTask.CompletedTask;
}
