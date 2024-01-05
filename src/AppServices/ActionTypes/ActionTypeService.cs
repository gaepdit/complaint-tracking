using AutoMapper;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.ActionTypes;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.ActionTypes;

public sealed class ActionTypeService(
    IActionTypeRepository repository,
    IActionTypeManager manager,
    IMapper mapper,
    IUserService userService)
    : IActionTypeService
{
    public async Task<ActionTypeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await repository.FindAsync(id, token);
        return mapper.Map<ActionTypeUpdateDto>(item);
    }

    public async Task<IReadOnlyList<ActionTypeViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return mapper.Map<List<ActionTypeViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await repository.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateAsync(string name, CancellationToken token = default)
    {
        var item = await manager.CreateAsync(name, (await userService.GetCurrentUserAsync())?.Id, token);
        await repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task UpdateAsync(Guid id, ActionTypeUpdateDto resource, CancellationToken token = default)
    {
        var item = await repository.GetAsync(id, token);

        if (item.Name != resource.Name.Trim())
            await manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;
        item.SetUpdater((await userService.GetCurrentUserAsync())?.Id);

        await repository.UpdateAsync(item, token: token);
    }

    public void Dispose() => repository.Dispose();
    public ValueTask DisposeAsync() => repository.DisposeAsync();
}
