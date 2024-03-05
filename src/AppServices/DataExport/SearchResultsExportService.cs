using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.Domain.DataViews;
using Cts.Domain.Entities.Complaints;
using GaEpd.AppLibrary.Extensions;

namespace Cts.AppServices.DataExport;

public sealed class SearchResultsExportService(IComplaintRepository complaintRepository, IDataViewRepository dataViews)
    : ISearchResultsExportService
{
    public async Task<int> CountAsync(ComplaintSearchDto spec, CancellationToken token) =>
        await complaintRepository.CountAsync(ComplaintFilters.SearchPredicate(spec), token).ConfigureAwait(false);

    public async Task<IReadOnlyList<SearchResultsExportDto>> ExportSearchResultsAsync(ComplaintSearchDto spec,
        CancellationToken token) =>
        (await complaintRepository.GetListWithMostRecentActionAsync(ComplaintFilters.SearchPredicate(spec),
            sorting: spec.Sort.GetDescription(), token).ConfigureAwait(false))
        .Select(complaint => new SearchResultsExportDto(complaint)).ToList();

    #region IDisposable,  IAsyncDisposable

    void IDisposable.Dispose()
    {
        complaintRepository.Dispose();
        dataViews.Dispose();
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        var complaintRepositoryTask = complaintRepository.DisposeAsync();
        var dataViewsTasks = dataViews.DisposeAsync();
        await complaintRepositoryTask.ConfigureAwait(false);
        await dataViewsTasks.ConfigureAwait(false);
    }

    #endregion
}
