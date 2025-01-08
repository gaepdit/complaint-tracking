using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Complaints.QueryDto;

namespace Cts.AppServices.DataExport;

public interface ISearchResultsExportService : IDisposable, IAsyncDisposable
{
    Task<int> CountComplaintsAsync(ComplaintSearchDto spec, CancellationToken token);

    Task<IReadOnlyList<ComplaintExportDto>> ExportComplaintsAsync(ComplaintSearchDto spec,
        CancellationToken token);

    Task<int> CountActionsAsync(ActionSearchDto spec, CancellationToken token);

    Task<IReadOnlyList<ActionExportDto>> ExportActionsAsync(ActionSearchDto spec,
        CancellationToken token);
}
