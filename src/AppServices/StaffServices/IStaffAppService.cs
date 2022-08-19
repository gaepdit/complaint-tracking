using Microsoft.AspNetCore.Identity;

namespace Cts.AppServices.StaffServices;

public interface IStaffAppService : IDisposable
{
    Task<StaffUpdateDto?> FindForUpdateAsync(Guid id);
    public Task<List<StaffViewDto>> FindUsersAsync(StaffSearchDto filter);
    public Task<IList<string>> GetUserRolesAsync(string id);
    public Task<IdentityResult> UpdateUserRolesAsync(string id, Dictionary<string, bool> roleUpdates);
    Task<IdentityResult> UpdateAsync(StaffUpdateDto resource);
}
