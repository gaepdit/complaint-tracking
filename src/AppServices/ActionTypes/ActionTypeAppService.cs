using AutoMapper;
using Cts.AppServices.Users;
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

    public async Task<ActionTypeUpdateDto?> FindForUpdateAsync(Guid id)
    {
        var actionType = await _repository.FindAsync(id);
        return _mapper.Map<ActionTypeUpdateDto>(actionType);
    }

    public async Task<IReadOnlyList<ActionTypeViewDto>> GetListAsync()
    {
        var actionTypes = await _repository.GetListAsync();
        return _mapper.Map<List<ActionTypeViewDto>>(actionTypes);
    }

    public async Task<ActionTypeViewDto> CreateAsync(string name)
    {
        // Create and insert the new item
        var actionType = await _manager.CreateAsync(name);
        actionType.SetCreator((await _userService.GetCurrentUserAsync())?.Id);

        await _repository.InsertAsync(actionType);

        // Return DTO
        return _mapper.Map<ActionTypeViewDto>(actionType);
    }

    public async Task UpdateAsync(ActionTypeUpdateDto resource)
    {
        var actionType = await _repository.GetAsync(resource.Id);
        if (actionType.Name != resource.Name.Trim()) 
            await _manager.ChangeNameAsync(actionType, resource.Name);
        actionType.Active = resource.Active;
        actionType.SetUpdater((await _userService.GetCurrentUserAsync())?.Id);

        await _repository.UpdateAsync(actionType);
    }

    public void Dispose() => _repository.Dispose();
}
