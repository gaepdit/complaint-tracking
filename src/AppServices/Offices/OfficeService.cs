using AutoMapper;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.Offices;

public sealed class OfficeService : IOfficeService
{
    private readonly IOfficeRepository _repository;
    private readonly IOfficeManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _users;

    public OfficeService(IOfficeRepository repository, IOfficeManager manager, IMapper mapper, IUserService users)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _users = users;
    }

    public async Task<OfficeWithAssignorDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.FindIncludeAssignorAsync(id, token);
        return _mapper.Map<OfficeWithAssignorDto>(item);
    }

    public async Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.FindIncludeAssignorAsync(id, token);
        return _mapper.Map<OfficeUpdateDto>(item);
    }

    public async Task<IReadOnlyList<OfficeWithAssignorDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await _repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<IReadOnlyList<OfficeWithAssignorDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await _repository.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default)
    {
        var item = await _manager.CreateAsync(resource.Name, (await _users.GetCurrentUserAsync())?.Id, token);

        if (resource.AssignorId != null) item.Assignor = await _users.FindUserAsync(resource.AssignorId);

        await _repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task UpdateAsync(Guid id, OfficeUpdateDto resource, CancellationToken token = default)
    {
        var item = await _repository.GetAsync(id, token);
        item.SetUpdater((await _users.GetCurrentUserAsync())?.Id);

        if (item.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;

        if (resource.AssignorId != null)
            item.Assignor = await _users.FindUserAsync(resource.AssignorId);

        await _repository.UpdateAsync(item, token: token);
    }

    public async Task<IReadOnlyList<ListItem<string>>> GetStaffListItemsAsync(
        Guid? id, bool activeOnly, CancellationToken token = default) =>
        id is null
            ? Array.Empty<ListItem<string>>()
            : (await _repository.GetActiveStaffMembersListAsync(id.Value, token))
            .Select(e => new ListItem<string>(e.Id, e.SortableNameWithInactive))
            .ToList();

    public async Task<bool> UserIsAssignorAsync(Guid id, string userId, CancellationToken token = default)
    {
        var office = await _repository.FindIncludeAssignorAsync(id, token);
        return office is { Active: true } && office.Assignor?.Id == userId;
    }

    public void Dispose() => _repository.Dispose();
}
