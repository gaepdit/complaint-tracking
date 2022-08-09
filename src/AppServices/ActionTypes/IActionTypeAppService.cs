using AutoMapper;
using Cts.Domain.Entities;

namespace Cts.AppServices.ActionTypes;

public interface IActionTypeAppService : IDisposable
{
    Task<ActionTypeViewDto> GetAsync(Guid id);
    Task<IReadOnlyList<ActionTypeViewDto>> GetListAsync();
    Task<ActionTypeViewDto> CreateAsync(string name);
    Task UpdateAsync(Guid id, ActionTypeUpdateDto resource);
}

public class ActionTypeViewDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
}

public class ActionTypeUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
}

public class ActionTypeMappingProfile : Profile
{
    public ActionTypeMappingProfile()
    {
        CreateMap<ActionType, ActionTypeViewDto>();
    }
}
