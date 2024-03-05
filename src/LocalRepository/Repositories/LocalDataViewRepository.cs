using Cts.Domain.DataViews;
using Cts.Domain.DataViews.DataArchiveViews;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalDataViewRepository : IDataViewRepository
{
    public Task<List<OpenComplaint>> OpenComplaintsAsync(CancellationToken token) =>
        throw new NotImplementedException();

    public Task<List<ClosedComplaint>> ClosedComplaintsAsync(CancellationToken token) =>
        throw new NotImplementedException();

    public Task<List<ClosedComplaintAction>> ClosedComplaintActionsAsync(CancellationToken token) =>
        throw new NotImplementedException();

    public Task<List<RecordsCount>> RecordsCountAsync(CancellationToken token) =>
        throw new NotImplementedException();

    void IDisposable.Dispose()
    {
        // Method intentionally left empty.
    }

    ValueTask IAsyncDisposable.DisposeAsync() => ValueTask.CompletedTask;
}
