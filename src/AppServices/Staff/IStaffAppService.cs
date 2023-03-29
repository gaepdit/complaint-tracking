using Cts.Domain.Security;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Identity;

namespace Cts.AppServices.Staff;

public interface IStaffAppService : IDisposable
{
    Task<StaffViewDto> GetCurrentUserAsync();
    Task<StaffViewDto> GetAsync(string id);
    Task<StaffViewDto?> FindAsync(string id);
    public Task<List<StaffViewDto>> GetListAsync(StaffSearchDto filter);
    Task<IReadOnlyList<ListItem<string>>> GetActiveStaffMembersAsync();
    Task<IReadOnlyList<ListItem<string>>> GetAllStaffMembersAsync();
    public Task<IList<string>> GetRolesAsync(string id);
    public Task<IList<AppRole>> GetAppRolesAsync(string id);
    public Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles);
    Task<IdentityResult> UpdateAsync(StaffUpdateDto resource);
}
