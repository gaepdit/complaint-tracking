using Cts.Domain.DataViews;
using Cts.Domain.DataViews.ReportingViews;

namespace Cts.AppServices.Reporting;

public sealed class ReportingService(IDataViewRepository dataViewRepository) : IReportingService
{
    public Task<List<StaffViewWithComplaints>> DaysSinceLastActionAsync(Guid officeId, int threshold) =>
        dataViewRepository.DaysSinceLastActionAsync(officeId, threshold);

    public Task<List<ComplaintView>> ComplaintsAssignedToInactiveUsersAsync(Guid officeId) =>
        dataViewRepository.ComplaintsAssignedToInactiveUsersAsync(officeId);

    public Task<List<StaffViewWithComplaints>> DaysToClosureByStaffAsync(Guid officeId, DateOnly dateFrom,
        DateOnly dateTo, bool includeAdminClosed) =>
        dataViewRepository.DaysToClosureByStaffAsync(officeId, dateFrom, dateTo, includeAdminClosed);

    public void Dispose() => dataViewRepository.Dispose();
    public ValueTask DisposeAsync() => dataViewRepository.DisposeAsync();
}
