using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.Domain.Entities.Complaints;
using GaEpd.AppLibrary.Extensions;

namespace Cts.AppServices.DataExport;

public sealed class DataExportService(IComplaintRepository complaintRepository) : IDataExportService
{
    public async Task<int> CountAsync(ComplaintSearchDto spec, CancellationToken token = default) =>
        await complaintRepository.CountAsync(ComplaintFilters.SearchPredicate(spec), token).ConfigureAwait(false);

    public async Task<IReadOnlyList<ComplaintExportDto>> ExportSearchAsync(ComplaintSearchDto spec,
        CancellationToken token = default) =>
        (await complaintRepository.GetListWithMostRecentActionAsync(ComplaintFilters.SearchPredicate(spec),
            sorting: spec.Sort.GetDescription(), token).ConfigureAwait(false))
        .Select(complaint => new ComplaintExportDto(complaint)).ToList();

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => complaintRepository.Dispose();

    public async ValueTask DisposeAsync() => await complaintRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
