using AutoMapper;
using Cts.AppServices.UserServices;
using Cts.Domain.Concerns;

namespace Cts.AppServices.Concerns;

public sealed class ConcernAppService : IConcernAppService
{
    private readonly IConcernRepository _repository;
    private readonly IConcernManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public ConcernAppService(
        IConcernRepository repository,
        IConcernManager manager,
        IMapper mapper,
        IUserService userService)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<ConcernUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var concern = await _repository.FindAsync(id, token);
        return _mapper.Map<ConcernUpdateDto>(concern);
    }

    public async Task<IReadOnlyList<ConcernViewDto>> GetListAsync(CancellationToken token = default)
    {
        var concerns = (await _repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<List<ConcernViewDto>>(concerns);
    }

    public async Task<Guid> CreateAsync(string name, CancellationToken token = default)
    {
        var concern = await _manager.CreateAsync(name, token);
        concern.SetCreator((await _userService.GetCurrentUserAsync())?.Id);
        await _repository.InsertAsync(concern, token: token);
        return concern.Id;
    }

    public async Task UpdateAsync(ConcernUpdateDto resource, CancellationToken token = default)
    {
        var concern = await _repository.GetAsync(resource.Id, token);

        if (concern.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(concern, resource.Name, token);
        concern.Active = resource.Active;
        concern.SetUpdater((await _userService.GetCurrentUserAsync())?.Id);

        await _repository.UpdateAsync(concern, token: token);
    }

    public void Dispose() => _repository.Dispose();
}
