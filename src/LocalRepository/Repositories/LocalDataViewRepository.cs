using Cts.Domain.DataViews;
using Cts.Domain.DataViews.DataArchiveViews;
using Cts.Domain.DataViews.ReportingViews;
using Cts.TestData.DataViews;

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
    public Task<List<StaffReportView>> ComplaintsAssignedToInactiveUsersAsync() =>
        Task.FromResult(DataViewsTestData.GetStaffReportData());

    public Task<List<StaffReportView>> ComplaintsByStaffAsync(Guid officeId, DateOnly dateFrom, DateOnly dateTo) => 
        Task.FromResult(DataViewsTestData.GetStaffReportData());

    public Task<List<StaffReportView>> DaysSinceMostRecentActionAsync(Guid officeId, int threshold) =>
        Task.FromResult(DataViewsTestData.GetStaffReportData());

    public Task<List<OfficeReportView>> DaysToClosureByOfficeAsync(DateOnly dateFrom, DateOnly dateTo,
        bool includeAdminClosed) => Task.FromResult(DataViewsTestData.GetOfficeReportData());

    public Task<List<StaffReportView>> DaysToClosureByStaffAsync(Guid officeId, DateOnly dateFrom,
        DateOnly dateTo, bool includeAdminClosed) => Task.FromResult(DataViewsTestData.GetStaffReportData());

    public Task<List<StaffReportView>> DaysToFollowupByStaffAsync(Guid officeId, DateOnly dateFrom,
        DateOnly dateTo) => Task.FromResult(DataViewsTestData.GetStaffReportData());

    void IDisposable.Dispose()
    {
        // Method intentionally left empty.
    }

    ValueTask IAsyncDisposable.DisposeAsync() => ValueTask.CompletedTask;
}
