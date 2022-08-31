using Cts.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Cts.AppServices.StaffServices;

public interface IStaffAppService : IDisposable
{
    Task<StaffViewDto?> FindAsync(Guid id);
    public Task<List<StaffViewDto>> GetListAsync(StaffSearchDto filter);
    public Task<IList<string>> GetRolesAsync(Guid id);
    public Task<IList<CtsRole>> GetCtsRolesAsync(Guid id);
    public Task<IdentityResult> UpdateRolesAsync(Guid id, Dictionary<string, bool> roles);
    Task<IdentityResult> UpdateAsync(StaffUpdateDto resource);
}
