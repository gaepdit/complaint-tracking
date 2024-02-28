using Cts.AppServices.Complaints.QueryDto;

namespace Cts.AppServices.DataExport;

public interface IDataExportService : IDisposable, IAsyncDisposable
{
    Task<int> CountAsync(ComplaintSearchDto spec, CancellationToken token = default);

    Task<IReadOnlyList<ComplaintExportDto>> ExportSearchAsync(ComplaintSearchDto spec,
        CancellationToken token = default);
}
