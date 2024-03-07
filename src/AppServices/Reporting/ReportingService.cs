using Cts.Domain.DataViews;
using Cts.Domain.DataViews.ReportingViews;

namespace Cts.AppServices.Reporting;

public sealed class ReportingService(IDataViewRepository dataViewRepository) : IReportingService
{
    public Task<List<StaffViewWithComplaints>> DaysSinceLastActionAsync(Guid officeId, int ageThreshold,
        CancellationToken token) => dataViewRepository.DaysSinceLastActionAsync(officeId, ageThreshold, token);

    public void Dispose() => dataViewRepository.Dispose();
    public ValueTask DisposeAsync() => dataViewRepository.DisposeAsync();
}
