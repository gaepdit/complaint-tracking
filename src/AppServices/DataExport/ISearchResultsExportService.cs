using Cts.AppServices.Complaints.QueryDto;

namespace Cts.AppServices.DataExport;

public interface ISearchResultsExportService : IDisposable, IAsyncDisposable
{
    Task<int> CountAsync(ComplaintSearchDto spec, CancellationToken token);

    Task<IReadOnlyList<SearchResultsExportDto>> ExportSearchResultsAsync(ComplaintSearchDto spec,
        CancellationToken token);
}
