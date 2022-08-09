using AutoMapper;
using Cts.AppServices.Users;
using Cts.Domain.Entities;
using Cts.Domain.Users;

namespace Cts.AppServices.Offices;

public interface IOfficeAppService : IDisposable
{
    Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<OfficeViewDto>> GetListAsync(CancellationToken token = default);
    Task<OfficeViewDto> CreateAsync(OfficeCreateDto resource, CancellationToken token = default);
    Task UpdateAsync(OfficeUpdateDto resource, CancellationToken token = default);
    Task<IReadOnlyList<UserViewDto>> GetUsersAsync(Guid id, CancellationToken token = default);
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
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ApplicationUser? MasterUser { get; set; }
    public bool Active { get; set; }
}

public class OfficeMappingProfile : Profile
{
    public OfficeMappingProfile()
    {
        CreateMap<Office, OfficeViewDto>();
        CreateMap<Office, OfficeUpdateDto>();
    }
}
