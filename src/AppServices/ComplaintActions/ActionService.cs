using AutoMapper;
using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.ComplaintActions;

public sealed class ActionService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization,
    IComplaintRepository complaintRepository,
    IComplaintManager complaintManager,
    IActionRepository actionRepository,
    IActionTypeRepository actionTypeRepository)
    : IActionService
{
    public async Task<Guid> CreateAsync(ActionCreateDto resource, CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(resource.ComplaintId, token: token).ConfigureAwait(false);
        var actionItemType = await actionTypeRepository.GetAsync(resource.ActionTypeId!.Value, token: token)
            .ConfigureAwait(false);

        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var action = complaintManager.CreateAction(complaint, actionItemType, currentUser);

        action.ActionDate = resource.ActionDate!.Value;
        action.Investigator = resource.Investigator;
        action.Comments = resource.Comments;

        await actionRepository.InsertAsync(action, token: token).ConfigureAwait(false);
        return action.Id;
    }

    public async Task<ActionViewDto?> FindAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ActionViewDto>(await actionRepository.FindIncludeAllAsync(action => action.Id == id, token: token)
            .ConfigureAwait(false));

    public async Task<ActionUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ActionUpdateDto>(
            await actionRepository.FindIncludeAllAsync(action => action.Id == id && !action.IsDeleted, token: token)
                .ConfigureAwait(false));

    public async Task<IPaginatedResult<ActionSearchResultDto>> SearchAsync(ActionSearchDto spec,
        PaginatedRequest paging, CancellationToken token = default)
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.DivisionManager).ConfigureAwait(false))
            spec.DeletedStatus = null;

        var predicate = ActionFilters.SearchPredicate(spec);
        var count = await actionRepository.CountAsync(predicate, token: token).ConfigureAwait(false);
        string[] includeProperties = spec.DeletedStatus is null ? [] : ["Complaint"];
        var actions = await actionRepository
            .GetPagedListAsync(predicate, paging, includeProperties, token: token)
            .ConfigureAwait(false);
        var list = count > 0 ? mapper.Map<IReadOnlyList<ActionSearchResultDto>>(actions) : [];

        return new PaginatedResult<ActionSearchResultDto>(list, count, paging);
    }

    public async Task UpdateAsync(Guid id, ActionUpdateDto resource, CancellationToken token = default)
    {
        var action = await actionRepository.GetAsync(id, token: token).ConfigureAwait(false);
        action.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        action.ActionType = await actionTypeRepository.GetAsync(resource.ActionTypeId!.Value, token: token)
            .ConfigureAwait(false);
        action.ActionDate = resource.ActionDate!.Value;
        action.Investigator = resource.Investigator;
        action.Comments = resource.Comments;

        await actionRepository.UpdateAsync(action, token: token).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid actionItemId, CancellationToken token = default)
    {
        var action = await actionRepository.GetAsync(actionItemId, token: token).ConfigureAwait(false);
        action.SetDeleted((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await actionRepository.UpdateAsync(action, token: token).ConfigureAwait(false);
    }

    public async Task RestoreAsync(Guid actionItemId, CancellationToken token = default)
    {
        var action = await actionRepository.GetAsync(actionItemId, token: token).ConfigureAwait(false);
        action.SetNotDeleted();
        await actionRepository.UpdateAsync(action, token: token).ConfigureAwait(false);
    }

    public void Dispose()
    {
        complaintRepository.Dispose();
        actionRepository.Dispose();
        actionTypeRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await complaintRepository.DisposeAsync().ConfigureAwait(false);
        await actionRepository.DisposeAsync().ConfigureAwait(false);
        await actionTypeRepository.DisposeAsync().ConfigureAwait(false);
    }
}
