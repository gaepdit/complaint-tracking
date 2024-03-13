using Cts.Domain.DataViews;
using Cts.Domain.DataViews.ReportingViews;

namespace Cts.AppServices.Reporting;

public sealed class ReportingService(IDataViewRepository dataViewRepository) : IReportingService
{
    public Task<List<StaffViewWithComplaints>> DaysSinceMostRecentActionAsync(Guid officeId, int threshold) =>
        dataViewRepository.DaysSinceMostRecentActionAsync(officeId, threshold);

    public Task<List<ComplaintView>> ComplaintsAssignedToInactiveUsersAsync(Guid officeId) =>
        dataViewRepository.ComplaintsAssignedToInactiveUsersAsync(officeId);

    public Task<List<StaffViewWithComplaints>> DaysToClosureByStaffAsync(Guid officeId, DateOnly dateFrom,
        DateOnly dateTo, bool includeAdminClosed) =>
        dataViewRepository.DaysToClosureByStaffAsync(officeId, dateFrom, dateTo, includeAdminClosed);

    public Task<List<StaffViewWithComplaints>> DaysToFollowupByStaffAsync(Guid office, DateOnly dateFrom,
        DateOnly dateTo) => dataViewRepository.DaysToFollowupByStaffAsync(office, dateFrom, dateTo);

    public void Dispose() => dataViewRepository.Dispose();
    public ValueTask DisposeAsync() => dataViewRepository.DisposeAsync();
}
