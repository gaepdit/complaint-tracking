using AutoMapper;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.Offices;

public sealed class OfficeService(
    IOfficeRepository repository,
    IOfficeManager manager,
    IMapper mapper,
    IUserService users)
    : IOfficeService
{
    public async Task<OfficeWithAssignorDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var item = await repository.FindIncludeAssignorAsync(id, token);
        return mapper.Map<OfficeWithAssignorDto>(item);
    }

    public async Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<OfficeUpdateDto>(await repository.FindIncludeAssignorAsync(id, token));

    public async Task<IReadOnlyList<OfficeWithAssignorDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await repository.GetListIncludeAssignorAsync(token)).OrderBy(e => e.Name).ToList();
        return mapper.Map<IReadOnlyList<OfficeWithAssignorDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await repository.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default)
    {
        var item = await manager.CreateAsync(resource.Name, (await users.GetCurrentUserAsync())?.Id, token);

        if (resource.AssignorId != null) item.Assignor = await users.FindUserAsync(resource.AssignorId);

        await repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task UpdateAsync(Guid id, OfficeUpdateDto resource, CancellationToken token = default)
    {
        var item = await repository.GetAsync(id, token);
        item.SetUpdater((await users.GetCurrentUserAsync())?.Id);

        if (item.Name != resource.Name.Trim())
            await manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;

        if (resource.AssignorId != null)
            item.Assignor = await users.FindUserAsync(resource.AssignorId);

        await repository.UpdateAsync(item, token: token);
    }

    public async Task<IReadOnlyList<ListItem<string>>> GetStaffListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default) =>
        id is null
            ? Array.Empty<ListItem<string>>()
            : (await repository.GetStaffMembersListAsync(id.Value, includeInactive, token))
            .Select(e => new ListItem<string>(e.Id, e.SortableNameWithInactive))
            .ToList();

    public async Task<bool> UserIsAssignorAsync(Guid id, string userId, CancellationToken token = default)
    {
        var office = await repository.FindIncludeAssignorAsync(id, token);
        return office is { Active: true } && office.Assignor?.Id == userId;
    }

    public void Dispose() => repository.Dispose();
    public ValueTask DisposeAsync() => repository.DisposeAsync();
}
