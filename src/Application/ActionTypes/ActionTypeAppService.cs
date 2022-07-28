using AutoMapper;
using Cts.Domain.ActionTypes;

namespace Cts.Application.ActionTypes;

public sealed class ActionTypeAppService : IActionTypeAppService
{
    private readonly IActionTypeRepository _repository;
    private readonly IActionTypeManager _manager;
    private readonly IMapper _mapper;

    public ActionTypeAppService(
        IActionTypeRepository repository,
        IActionTypeManager manager,
        IMapper mapper)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
    }

    public async Task<ActionTypeViewDto> GetAsync(Guid id)
    {
        var actionType = await _repository.GetAsync(id);
        return _mapper.Map<ActionTypeViewDto>(actionType);
    }

    public async Task<IReadOnlyList<ActionTypeViewDto>> GetListAsync()
    {
        var actionTypes = await _repository.GetListAsync();
        return _mapper.Map<List<ActionTypeViewDto>>(actionTypes);
    }

    public async Task<ActionTypeViewDto> CreateAsync(ActionTypeCreateDto resource)
    {
        // Create and insert the new item
        var actionType = await _manager.CreateAsync(resource.Name);
        await _repository.InsertAsync(actionType);

        // Return DTO
        return _mapper.Map<ActionTypeViewDto>(actionType);
    }

    public async Task UpdateAsync(Guid id, ActionTypeUpdateDto resource)
    {
        var actionType = await _repository.GetAsync(id);

        if (actionType.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(actionType, resource.Name);

        actionType.Active = resource.Active;

        await _repository.UpdateAsync(actionType);
    }

    public void Dispose() => _repository.Dispose();
}
