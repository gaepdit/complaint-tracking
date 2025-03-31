using Cts.AppServices.ComplaintActions;
using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using GaEpd.AppLibrary.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.DataExport;

public sealed class SearchResultsExportService(
    IComplaintRepository complaintRepository,
    IActionRepository actionRepository,
    IUserService userService,
    IAuthorizationService authorization)
    : ISearchResultsExportService
{
    public async Task<int> CountComplaintsAsync(ComplaintSearchDto spec, CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.DivisionManager).ConfigureAwait(false))
            spec.DeletedStatus = null;

        return await complaintRepository.CountAsync(ComplaintFilters.SearchPredicate(spec), token: token)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<ComplaintExportDto>> ExportComplaintsAsync(ComplaintSearchDto spec,
        CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.DivisionManager).ConfigureAwait(false))
            spec.DeletedStatus = null;

        var results = await complaintRepository.GetListWithMostRecentActionAsync(ComplaintFilters.SearchPredicate(spec),
            sorting: spec.Sort.GetDescription(), token: token).ConfigureAwait(false);
        return results.Select(complaint => new ComplaintExportDto(complaint)).ToList();
    }

    public async Task<int> CountActionsAsync(ActionSearchDto spec, CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.DivisionManager).ConfigureAwait(false))
            spec.DeletedStatus = null;

        return await actionRepository.CountAsync(ActionFilters.SearchPredicate(spec), token: token)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<ActionExportDto>> ExportActionsAsync(ActionSearchDto spec, CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.DivisionManager).ConfigureAwait(false))
            spec.DeletedStatus = null;

        var results = await actionRepository.GetListAsync(ActionFilters.SearchPredicate(spec),
                ordering: spec.Sort.GetDescription(), includeProperties: ["Complaint"], token: token)
            .ConfigureAwait(false);
        return results.Select(action => new ActionExportDto(action)).ToList();
    }


    #region IDisposable,  IAsyncDisposable

    void IDisposable.Dispose() => complaintRepository.Dispose();
    async ValueTask IAsyncDisposable.DisposeAsync() => await complaintRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
