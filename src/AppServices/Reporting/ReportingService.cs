using Cts.Domain.DataViews;
using Cts.Domain.DataViews.ReportingViews;

namespace Cts.AppServices.Reporting;

public sealed class ReportingService(IDataViewRepository dataViewRepository) : IReportingService
{
    public Task<List<StaffViewWithComplaints>> DaysSinceLastActionAsync(Guid officeId, int threshold) =>
        dataViewRepository.DaysSinceLastActionAsync(officeId, threshold);

    public void Dispose() => dataViewRepository.Dispose();
    public ValueTask DisposeAsync() => dataViewRepository.DisposeAsync();
}
