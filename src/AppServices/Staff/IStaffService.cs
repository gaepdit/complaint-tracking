﻿using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Identity;

namespace Cts.AppServices.Staff;

public interface IStaffService : IDisposable, IAsyncDisposable
{
    Task<StaffViewDto> GetCurrentUserAsync();
    Task<StaffViewDto?> FindAsync(string id);
    Task<List<StaffViewDto>> GetListAsync(StaffSearchDto spec);
    Task<IPaginatedResult<StaffSearchResultDto>> SearchAsync(StaffSearchDto spec, PaginatedRequest paging);
    Task<IReadOnlyList<ListItem<string>>> GetStaffListItemsAsync(bool includeInactive = false);
    Task<IList<string>> GetRolesAsync(string id);
    Task<IList<AppRole>> GetAppRolesAsync(string id);
    Task<bool> HasAppRoleAsync(string id, AppRole role);
    Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles);
    Task<IdentityResult> UpdateAsync(string id, StaffUpdateDto resource);
}
