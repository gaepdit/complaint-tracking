using AutoMapper;
using Cts.Application.Users;
using Cts.Domain.Offices;
using Cts.Domain.Users;

namespace Cts.Application.Offices;

public interface IOfficeAppService : IDisposable
{
    Task<OfficeViewDto> GetAsync(Guid id);
    Task<IReadOnlyList<OfficeViewDto>> GetListAsync();
    Task<OfficeViewDto> CreateAsync(OfficeCreateDto resource);
    Task UpdateAsync(Guid id, OfficeUpdateDto resource);
    Task<IReadOnlyList<UserViewDto>> GetUsersAsync(Guid id);
}

public class OfficeViewDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
    public ApplicationUser? MasterUser { get; set; }
}

public class OfficeCreateDto
{
    public string Name { get; set; } = string.Empty;
    public ApplicationUser? MasterUser { get; set; }
}

public class OfficeUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public ApplicationUser? MasterUser { get; set; }
    public bool Active { get; set; }
}

public class OfficeMappingProfile : Profile
{
    public OfficeMappingProfile()
    {
        CreateMap<Office, OfficeViewDto>();
    }
}
