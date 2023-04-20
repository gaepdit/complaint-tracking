using Cts.Domain.Identity;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Identity;

namespace Cts.AppServices.Staff;

public interface IStaffAppService : IDisposable
{
    Task<StaffViewDto> GetCurrentUserAsync();
    Task<StaffViewDto?> FindCurrentUserAsync();
    Task<StaffViewDto> GetAsync(string id);
    Task<StaffViewDto?> FindAsync(string id);
    Task<List<StaffViewDto>> GetListAsync(StaffSearchDto filter);
    Task<IReadOnlyList<ListItem<string>>> GetStaffListItemsAsync(bool activeOnly);
    Task<IList<string>> GetRolesAsync(string id);
    Task<IList<AppRole>> GetAppRolesAsync(string id);
    Task<bool> HasAppRoleAsync(string id, AppRole role);
    Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles);
    Task<IdentityResult> UpdateAsync(StaffUpdateDto resource);
}
