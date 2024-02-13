using AutoMapper;
using Cts.AppServices.ServiceBase;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.Offices;

public sealed class OfficeService(
    IOfficeRepository repository,
    IOfficeManager manager,
    IMapper mapper,
    IUserService userService)
    : MaintenanceItemService<Office, OfficeViewDto, OfficeUpdateDto>
        (repository, manager, mapper, userService),
        IOfficeService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserService _userService = userService;

    // Hide the following base methods in order to include the assignor.
    public new async Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        _mapper.Map<OfficeUpdateDto>(await repository.FindIncludeAssignorAsync(id, token).ConfigureAwait(false));

    public new async Task UpdateAsync(Guid id, OfficeUpdateDto resource, CancellationToken token = default)
    {
        var office = await repository.GetAsync(id, token).ConfigureAwait(false);
        office.SetUpdater((await _userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        if (office.Name != resource.Name.Trim())
            await manager.ChangeNameAsync(office, resource.Name, token).ConfigureAwait(false);

        office.Active = resource.Active;

        if (resource.AssignorId != null)
            office.Assignor = await _userService.FindUserAsync(resource.AssignorId).ConfigureAwait(false);

        await repository.UpdateAsync(office, token: token).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<OfficeWithAssignorDto>> GetListIncludeAssignorAsync(
        CancellationToken token = default)
    {
        var list = await repository.GetListIncludeAssignorAsync(token).ConfigureAwait(false);
        return _mapper.Map<IReadOnlyList<OfficeWithAssignorDto>>(list);
    }

    public async Task<OfficeWithAssignorDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var office = await repository.FindIncludeAssignorAsync(id, token).ConfigureAwait(false);
        return _mapper.Map<OfficeWithAssignorDto>(office);
    }

    public async Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default)
    {
        var office = await manager.CreateAsync(resource.Name, (await _userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id, token).ConfigureAwait(false);
        if (resource.AssignorId != null) office.Assignor = await _userService.FindUserAsync(resource.AssignorId).ConfigureAwait(false);
        await repository.InsertAsync(office, token: token).ConfigureAwait(false);
        return office.Id;
    }

    public async Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default) =>
        id is null
            ? Array.Empty<ListItem<string>>()
            : (await repository.GetStaffMembersListAsync(id.Value, includeInactive, token).ConfigureAwait(false))
            .Select(e => new ListItem<string>(e.Id, e.SortableNameWithInactive))
            .ToList();

    public async Task<bool> UserIsAssignorAsync(Guid id, string userId, CancellationToken token = default)
    {
        var office = await repository.FindIncludeAssignorAsync(id, token).ConfigureAwait(false);
        return office is { Active: true } && office.Assignor?.Id == userId;
    }
}
