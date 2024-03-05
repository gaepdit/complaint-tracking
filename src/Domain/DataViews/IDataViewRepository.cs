using Cts.Domain.DataViews.DataArchiveViews;

namespace Cts.Domain.DataViews;

public interface IDataViewRepository : IDisposable, IAsyncDisposable
{
    Task<List<OpenComplaint>> OpenComplaintsAsync(CancellationToken token);
    Task<List<ClosedComplaint>> ClosedComplaintsAsync(CancellationToken token);
    Task<List<ClosedComplaintAction>> ClosedComplaintActionsAsync(CancellationToken token);
    Task<List<RecordsCount>> RecordsCountAsync(CancellationToken token);
}
