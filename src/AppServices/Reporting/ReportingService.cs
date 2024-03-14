using Cts.Domain.DataViews;
using Cts.Domain.DataViews.ReportingViews;

namespace Cts.AppServices.Reporting;

public sealed class ReportingService(IDataViewRepository dataViewRepository) : IReportingService
{
    public Task<List<StaffReportView>> ComplaintsAssignedToInactiveUsersAsync() =>
        dataViewRepository.ComplaintsAssignedToInactiveUsersAsync();

    public Task<List<StaffReportView>> ComplaintsByStaffAsync(Guid officeId, DateOnly dateFrom, DateOnly dateTo) => 
        dataViewRepository.ComplaintsByStaffAsync(officeId, dateFrom, dateTo);

    public Task<List<StaffReportView>> DaysSinceMostRecentActionAsync(Guid officeId, int threshold) =>
        dataViewRepository.DaysSinceMostRecentActionAsync(officeId, threshold);

    public Task<List<OfficeReportView>> DaysToClosureByOfficeAsync(DateOnly dateFrom, DateOnly dateTo,
        bool includeAdminClosed) => dataViewRepository.DaysToClosureByOfficeAsync(dateFrom, dateTo, includeAdminClosed);

    public Task<List<StaffReportView>> DaysToClosureByStaffAsync(Guid officeId, DateOnly dateFrom,
        DateOnly dateTo, bool includeAdminClosed) =>
        dataViewRepository.DaysToClosureByStaffAsync(officeId, dateFrom, dateTo, includeAdminClosed);

    public Task<List<StaffReportView>> DaysToFollowupByStaffAsync(Guid office, DateOnly dateFrom,
        DateOnly dateTo) => dataViewRepository.DaysToFollowupByStaffAsync(office, dateFrom, dateTo);

    public void Dispose() => dataViewRepository.Dispose();
    public ValueTask DisposeAsync() => dataViewRepository.DisposeAsync();
}
