using AutoMapper;
using Cts.Domain.Entities;

namespace Cts.AppServices.ActionTypes;

public interface IActionTypeAppService : IDisposable
{
    Task<ActionTypeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<ActionTypeViewDto>> GetListAsync(CancellationToken token = default);
    Task<ActionTypeViewDto> CreateAsync(string name, CancellationToken token = default);
    Task UpdateAsync(ActionTypeUpdateDto resource, CancellationToken token = default);
}

public class ActionTypeViewDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
}

public class ActionTypeUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
}

public class ActionTypeMappingProfile : Profile
{
    public ActionTypeMappingProfile()
    {
        CreateMap<ActionType, ActionTypeViewDto>();
        CreateMap<ActionType, ActionTypeUpdateDto>();
    }
}
