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
        var complaint = await complaintRepository.GetAsync(resource.ComplaintId, token);
        var actionItemType = await actionTypeRepository.GetAsync(resource.ActionTypeId!.Value, token);

        var currentUser = await userService.GetCurrentUserAsync();
        var action = complaintManager.AddAction(complaint, actionItemType, currentUser);

        action.ActionDate = resource.ActionDate;
        action.Investigator = resource.Investigator;
        action.Comments = resource.Comments;

        await actionRepository.InsertAsync(action, token: token);
        return action.Id;
    }

    public async Task<ComplaintActionViewDto?> FindAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ComplaintActionViewDto>(
            await actionRepository.FindAsync(action => action.Id == id && !action.IsDeleted, token));

    public async Task<ComplaintActionUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ComplaintActionUpdateDto>(
            await actionRepository.FindAsync(action => action.Id == id && !action.IsDeleted, token));

    public async Task UpdateAsync(Guid id, ComplaintActionUpdateDto resource, CancellationToken token = default)
    {
        var action = await actionRepository.GetAsync(id, token);
        action.SetUpdater((await userService.GetCurrentUserAsync())?.Id);

        action.ActionType = await actionTypeRepository.GetAsync(resource.ActionTypeId!.Value, token);
        action.ActionDate = resource.ActionDate;
        action.Investigator = resource.Investigator;
        action.Comments = resource.Comments;

        await actionRepository.UpdateAsync(action, token: token);
    }

    public async Task DeleteAsync(Guid actionItemId, CancellationToken token = default)
    {
        var action = await actionRepository.GetAsync(actionItemId, token);
        action.SetDeleted((await userService.GetCurrentUserAsync())?.Id);
        await actionRepository.UpdateAsync(action, token: token);
    }

    public void Dispose()
    {
        complaintRepository.Dispose();
        actionRepository.Dispose();
        actionTypeRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await complaintRepository.DisposeAsync();
        await actionRepository.DisposeAsync();
        await actionTypeRepository.DisposeAsync();
    }
}
