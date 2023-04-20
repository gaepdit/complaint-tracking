using AutoMapper;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.Offices;

public sealed class OfficeAppService : IOfficeAppService
{
    private readonly IOfficeRepository _offices;
    private readonly IOfficeManager _officeManager;
    private readonly IMapper _mapper;
    private readonly IUserService _users;

    public OfficeAppService(
        IOfficeRepository offices,
        IOfficeManager officeManager,
        IMapper mapper,
        IUserService users)
    {
        _offices = offices;
        _officeManager = officeManager;
        _mapper = mapper;
        _users = users;
    }

    public async Task<OfficeAdminViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var item = await _offices.FindIncludeAssignorAsync(id, token);
        return _mapper.Map<OfficeAdminViewDto>(item);
    }

    public async Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await _offices.FindIncludeAssignorAsync(id, token);
        return _mapper.Map<OfficeUpdateDto>(item);
    }

    public async Task<IReadOnlyList<OfficeAdminViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await _offices.GetListIncludeAssignorAsync(token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<IReadOnlyList<OfficeAdminViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await _offices.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default)
    {
        var item = await _officeManager.CreateAsync(resource.Name, token);
        item.SetCreator((await _users.GetCurrentUserAsync())?.Id);

        if (resource.AssignorId != null) item.Assignor = await _users.FindUserAsync(resource.AssignorId);

        await _offices.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task UpdateAsync(OfficeUpdateDto resource, CancellationToken token = default)
    {
        var item = await _offices.GetAsync(resource.Id, token);
        item.Active = resource.Active;

        if (item.Name != resource.Name.Trim())
            await _officeManager.ChangeNameAsync(item, resource.Name, token);
        
        if (resource.AssignorId != null)
            item.Assignor = await _users.FindUserAsync(resource.AssignorId);

        item.SetUpdater((await _users.GetCurrentUserAsync())?.Id);
        await _offices.UpdateAsync(item, token: token);
    }

    public async Task<IReadOnlyList<ListItem<string>>> GetStaffListItemsAsync(
        Guid? id, bool activeOnly, CancellationToken token = default) =>
        id is null
            ? Array.Empty<ListItem<string>>()
            : (await _offices.GetStaffMembersListAsync(id.Value, activeOnly, token))
            .Select(e => new ListItem<string>(e.Id, e.SortableNameWithInactive))
            .ToList();

    public async Task<bool> UserIsAssignorAsync(Guid id, string userId, CancellationToken token = default)
    {
        var office = await _offices.FindIncludeAssignorAsync(id, token);
        return office is { Active: true } && office.Assignor?.Id == userId;
    }

    public void Dispose() => _offices.Dispose();
}
