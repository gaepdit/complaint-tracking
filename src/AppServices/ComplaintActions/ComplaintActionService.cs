using AutoMapper;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.Complaints;

namespace Cts.AppServices.ComplaintActions;

public sealed class ComplaintActionService(
    IMapper mapper,
    IUserService userService,
    IComplaintRepository complaintRepository,
    IComplaintManager complaintManager,
    IComplaintActionRepository actionRepository,
    IActionTypeRepository actionTypeRepository)
    : IComplaintActionService
{
    public async Task<Guid> CreateAsync(ComplaintActionCreateDto resource, CancellationToken token = default)
    {
        var complaint = await complaintRepository.GetAsync(resource.ComplaintId, token).ConfigureAwait(false);
        var actionItemType = await actionTypeRepository.GetAsync(resource.ActionTypeId, token)
            .ConfigureAwait(false);

        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var action = complaintManager.AddAction(complaint, actionItemType, currentUser);

        action.ActionDate = resource.ActionDate;
        action.Investigator = resource.Investigator;
        action.Comments = resource.Comments;

        await actionRepository.InsertAsync(action, token: token).ConfigureAwait(false);
        return action.Id;
    }

    public async Task<ComplaintActionViewDto?> FindAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ComplaintActionViewDto>(
            await actionRepository.FindAsync(id, token).ConfigureAwait(false));

    public async Task<ComplaintActionUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ComplaintActionUpdateDto>(
            await actionRepository.FindAsync(action => action.Id == id && !action.IsDeleted, token)
                .ConfigureAwait(false));

    public async Task UpdateAsync(Guid id, ComplaintActionUpdateDto resource, CancellationToken token = default)
    {
        var action = await actionRepository.GetAsync(id, token).ConfigureAwait(false);
        action.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        action.ActionType = await actionTypeRepository.GetAsync(resource.ActionTypeId, token)
            .ConfigureAwait(false);
        action.ActionDate = resource.ActionDate;
        action.Investigator = resource.Investigator;
        action.Comments = resource.Comments;

        await actionRepository.UpdateAsync(action, token: token).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid actionItemId, CancellationToken token = default)
    {
        var action = await actionRepository.GetAsync(actionItemId, token).ConfigureAwait(false);
        action.SetDeleted((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await actionRepository.UpdateAsync(action, token: token).ConfigureAwait(false);
    }

    public async Task RestoreAsync(Guid actionItemId, CancellationToken token = default)
    {
        var action = await actionRepository.GetAsync(actionItemId, token).ConfigureAwait(false);
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
