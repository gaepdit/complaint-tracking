using AutoMapper;
using Cts.AppServices.UserServices;
using Cts.Domain.ActionTypes;

namespace Cts.AppServices.ActionTypes;

public sealed class ActionTypeAppService : IActionTypeAppService
{
    private readonly IActionTypeRepository _repository;
    private readonly IActionTypeManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public ActionTypeAppService(
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
        var actionType = await _repository.FindAsync(id, token);
        return _mapper.Map<ActionTypeUpdateDto>(actionType);
    }

    public async Task<IReadOnlyList<ActionTypeViewDto>> GetListAsync(CancellationToken token = default)
    {
        var actionTypes = (await _repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<List<ActionTypeViewDto>>(actionTypes);
    }

    public async Task<Guid> CreateAsync(string name, CancellationToken token = default)
    {
        var actionType = await _manager.CreateAsync(name, token);
        actionType.SetCreator((await _userService.GetCurrentUserAsync())?.Id);
        await _repository.InsertAsync(actionType, token: token);
        return actionType.Id;
    }

    public async Task UpdateAsync(ActionTypeUpdateDto resource, CancellationToken token = default)
    {
        var actionType = await _repository.GetAsync(resource.Id, token);

        if (actionType.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(actionType, resource.Name, token);
        actionType.Active = resource.Active;
        actionType.SetUpdater((await _userService.GetCurrentUserAsync())?.Id);

        await _repository.UpdateAsync(actionType, token: token);
    }

    public void Dispose() => _repository.Dispose();
}
