using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using GaEpd.AppLibrary.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.DataExport;

public sealed class SearchResultsExportService(
    IComplaintRepository complaintRepository,
    IUserService userService,
    IAuthorizationService authorization)
    : ISearchResultsExportService
{
    public async Task<int> CountAsync(ComplaintSearchDto spec, CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.DivisionManager).ConfigureAwait(false))
            spec.DeletedStatus = null;

        return await complaintRepository.CountAsync(ComplaintFilters.SearchPredicate(spec), token)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<SearchResultsExportDto>> ExportSearchResultsAsync(ComplaintSearchDto spec,
        CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.DivisionManager).ConfigureAwait(false))
            spec.DeletedStatus = null;

        return (await complaintRepository.GetListWithMostRecentActionAsync(ComplaintFilters.SearchPredicate(spec),
                sorting: spec.Sort.GetDescription(), token).ConfigureAwait(false))
            .Select(complaint => new SearchResultsExportDto(complaint)).ToList();
    }

    #region IDisposable,  IAsyncDisposable

    void IDisposable.Dispose() => complaintRepository.Dispose();
    async ValueTask IAsyncDisposable.DisposeAsync() => await complaintRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
