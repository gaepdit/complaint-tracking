using AutoMapper;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.ActionTypes;
using GaEpd.AppLibrary.ListItems;

namespace Cts.AppServices.ActionTypes;

public sealed class ActionTypeService : IActionTypeService
{
    private readonly IActionTypeRepository _repository;
    private readonly IActionTypeManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public ActionTypeService(
        IActionTypeRepository repository,
        IActionTypeManager manager,
        IMapper mapper,
        IUserService userService)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<ActionTypeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.FindAsync(id, token);
        return _mapper.Map<ActionTypeUpdateDto>(item);
    }

    public async Task<IReadOnlyList<ActionTypeViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await _repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<List<ActionTypeViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await _repository.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateAsync(string name, CancellationToken token = default)
    {
        var item = await _manager.CreateAsync(name, (await _userService.GetCurrentUserAsync())?.Id, token);
        await _repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task UpdateAsync(ActionTypeUpdateDto resource, CancellationToken token = default)
    {
        var item = await _repository.GetAsync(resource.Id, token);

        if (item.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;
        item.SetUpdater((await _userService.GetCurrentUserAsync())?.Id);

        await _repository.UpdateAsync(item, token: token);
    }

    public void Dispose() => _repository.Dispose();
}
