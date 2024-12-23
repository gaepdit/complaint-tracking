using AutoMapper;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.Staff.Dto;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;

namespace Cts.AppServices.Staff;

public sealed class StaffService(
    IUserService userService,
    UserManager<ApplicationUser> userManager,
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IOfficeRepository officeRepository,
    IAuthorizationService authorization)
    : IStaffService
{
    public async Task<StaffViewDto> GetCurrentUserAsync()
    {
        var user = await userService.GetCurrentUserAsync().ConfigureAwait(false)
                   ?? throw new CurrentUserNotFoundException();
        return mapper.Map<StaffViewDto>(user);
    }

    public async Task<StaffViewDto?> FindAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id).ConfigureAwait(false);
        return mapper.Map<StaffViewDto?>(user);
    }

    public async Task<IReadOnlyList<StaffViewDto>> GetListAsync(StaffSearchDto spec)
    {
        var users = string.IsNullOrEmpty(spec.Role)
            ? userManager.Users.ApplyFilter(spec)
            : (await userManager.GetUsersInRoleAsync(spec.Role).ConfigureAwait(false)).AsQueryable().ApplyFilter(spec);

        return mapper.Map<IReadOnlyList<StaffViewDto>>(users);
    }

    public async Task<IPaginatedResult<StaffSearchResultDto>> SearchAsync(StaffSearchDto spec, PaginatedRequest paging)
    {
        var users = string.IsNullOrEmpty(spec.Role)
            ? userManager.Users.ApplyFilter(spec)
            : (await userManager.GetUsersInRoleAsync(spec.Role).ConfigureAwait(false)).AsQueryable().ApplyFilter(spec);
        var list = users.Skip(paging.Skip).Take(paging.Take);
        var listMapped = mapper.Map<IReadOnlyList<StaffSearchResultDto>>(list);

        return new PaginatedResult<StaffSearchResultDto>(listMapped, users.Count(), paging);
    }

    public Task<IReadOnlyList<ListItem<string>>> GetAsListItemsAsync(bool includeInactive = false)
    {
        var status = includeInactive ? SearchStaffStatus.All : SearchStaffStatus.Active;
        var spec = new StaffSearchDto(SortBy.NameAsc, null, null, null, null, status);
        var users = userManager.Users.ApplyFilter(spec);
        return Task.FromResult<IReadOnlyList<ListItem<string>>>(mapper.Map<IReadOnlyList<StaffViewDto>>(users)
            .Select(e => new ListItem<string>(e.Id, e.SortableNameWithOffice))
            .ToList());
    }

    public async Task<IReadOnlyList<ListItem<string>>> GetUsersInRoleAsListItemsAsync(AppRole role, Guid officeId) =>
        (await userManager.GetUsersInRoleAsync(role.Name).ConfigureAwait(false))
        .AsQueryable()
        .ApplyFilter(new StaffSearchDto(SortBy.NameAsc, null, null, role.Name, officeId, SearchStaffStatus.Active))
        .Select(user => new ListItem<string>(user.Id, user.SortableFullName))
        .ToList();

    public async Task<IList<string>> GetRolesAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id).ConfigureAwait(false);
        if (user is null) return new List<string>();
        return await userManager.GetRolesAsync(user).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<AppRole>> GetAppRolesAsync(string id) =>
        AppRole.RolesAsAppRoles(await GetRolesAsync(id).ConfigureAwait(false))
            .OrderBy(r => r.DisplayName).ToList();

    public async Task<bool> HasAppRoleAsync(string id, AppRole role)
    {
        var user = await userManager.FindByIdAsync(id).ConfigureAwait(false);
        if (user is null) return false;
        return await userManager.IsInRoleAsync(user, role.Name).ConfigureAwait(false);
    }

    public async Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles)
    {
        var principal = userService.GetCurrentPrincipal()!;
        if (!await authorization.Succeeded(principal, Policies.UserAdministrator).ConfigureAwait(false))
            throw new InsufficientPermissionsException(nameof(Policies.UserAdministrator));

        var filteredRoles =
            await authorization.Succeeded(principal, Policies.SuperUserAdministrator).ConfigureAwait(false)
                ? roles
                : roles.Where(pair => pair.Key != RoleName.DivisionManager && pair.Key != RoleName.SuperUserAdmin);

        var user = await userManager.FindByIdAsync(id).ConfigureAwait(false)
                   ?? throw new EntityNotFoundException<ApplicationUser>(id);

        foreach (var (role, value) in filteredRoles)
        {
            var result = await UpdateUserRoleAsync(user, role, value).ConfigureAwait(false);
            if (result != IdentityResult.Success) return result;
        }

        return IdentityResult.Success;

        async Task<IdentityResult> UpdateUserRoleAsync(ApplicationUser u, string r, bool addToRole)
        {
            var isInRole = await userManager.IsInRoleAsync(u, r).ConfigureAwait(false);
            if (addToRole == isInRole) return IdentityResult.Success;

            return addToRole switch
            {
                true => await userManager.AddToRoleAsync(u, r).ConfigureAwait(false),
                false => await userManager.RemoveFromRoleAsync(u, r).ConfigureAwait(false),
            };
        }
    }

    public async Task<IdentityResult> UpdateAsync(string id, StaffUpdateDto resource)
    {
        var principal = userService.GetCurrentPrincipal()!;
        if (id != principal.GetNameIdentifierId() &&
            !await authorization.Succeeded(principal, Policies.UserAdministrator).ConfigureAwait(false))
        {
            throw new InsufficientPermissionsException(nameof(Policies.UserAdministrator));
        }

        var user = await userManager.FindByIdAsync(id).ConfigureAwait(false)
                   ?? throw new EntityNotFoundException<ApplicationUser>(id);

        user.PhoneNumber = resource.PhoneNumber;
        user.Office = resource.OfficeId is null
            ? null
            : await officeRepository.GetAsync(resource.OfficeId.Value).ConfigureAwait(false);
        user.Active = resource.Active;
        user.ProfileUpdatedAt = DateTimeOffset.UtcNow;

        return await userManager.UpdateAsync(user).ConfigureAwait(false);
    }

    public void Dispose()
    {
        userManager.Dispose();
        officeRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        var task = officeRepository.DisposeAsync();
        userManager.Dispose();
        await task.ConfigureAwait(false);
    }
}
