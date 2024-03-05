using Cts.Domain.DataViews;
using Cts.Domain.DataViews.DataArchiveViews;

namespace Cts.EfRepository.Repositories;

public sealed class DataViewRepository(AppDbContext context) : IDataViewRepository
{
    public Task<List<OpenComplaint>> OpenComplaintsAsync(CancellationToken token) =>
        context.OpenComplaintsView.ToListAsync(cancellationToken: token);

    public Task<List<ClosedComplaint>> ClosedComplaintsAsync(CancellationToken token) =>
        context.ClosedComplaintsView.ToListAsync(cancellationToken: token);

    public Task<List<ClosedComplaintAction>> ClosedComplaintActionsAsync(CancellationToken token) =>
        context.ClosedComplaintActionsView.ToListAsync(cancellationToken: token);

    public Task<List<RecordsCount>> RecordsCountAsync(CancellationToken token) =>
        context.RecordsCountView.OrderBy(recordsCount => recordsCount.Order).ToListAsync(cancellationToken: token);

    #region IDisposable,  IAsyncDisposable

    void IDisposable.Dispose() => context.Dispose();
    async ValueTask IAsyncDisposable.DisposeAsync() => await context.DisposeAsync().ConfigureAwait(false);

    #endregion
}
