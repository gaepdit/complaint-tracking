using AutoMapper;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.Offices;

public sealed class OfficeAppService : IOfficeAppService
{
    private readonly IOfficeRepository _repository;
    private readonly IOfficeManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public OfficeAppService(
        IOfficeRepository repository,
        IOfficeManager manager,
        IMapper mapper,
        IUserService userService)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<OfficeAdminViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.FindAsync(id, token);
        return _mapper.Map<OfficeAdminViewDto>(item);
    }

    public async Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.FindAsync(id, token);
        return _mapper.Map<OfficeUpdateDto>(item);
    }

    public async Task<IReadOnlyList<OfficeAdminViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await _repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<IReadOnlyList<OfficeAdminViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await _repository.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default)
    {
        var item = await _manager.CreateAsync(resource.Name, resource.AssignorId, token);
        item.SetCreator((await _userService.GetCurrentUserAsync())?.Id);

        await _repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task UpdateAsync(OfficeUpdateDto resource, CancellationToken token = default)
    {
        var item = await _repository.GetAsync(resource.Id, token);

        if (item.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;
        item.AssignorId = resource.AssignorId;

        item.SetUpdater((await _userService.GetCurrentUserAsync())?.Id);
        await _repository.UpdateAsync(item, token: token);
    }

    public async Task<IReadOnlyList<ListItem<string>>> GetStaffListItemsAsync(
        Guid? id, bool activeOnly, CancellationToken token = default) =>
        id is null
            ? Array.Empty<ListItem<string>>()
            : (await _repository.GetStaffMembersListAsync(id.Value, activeOnly, token))
            .Select(e => new ListItem<string>(e.Id, e.SortableNameWithInactive))
            .ToList();

    public void Dispose() => _repository.Dispose();
}
