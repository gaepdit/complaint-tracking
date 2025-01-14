using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;

namespace Cts.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.UserAdministrator))]
public class EditRolesModel(IStaffService staffService, IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public string UserId { get; set; } = string.Empty;

    [BindProperty]
    public List<RoleSetting> RoleSettings { get; set; } = [];

    public StaffViewDto DisplayStaff { get; private set; } = null!;
    public string? OfficeName => DisplayStaff.Office?.Name;
    public bool CanEditWithElevatedPrivilege { get; private set; }

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null) return RedirectToPage("Index");
        var staff = await staffService.FindAsync(id);
        if (staff is null) return NotFound();

        DisplayStaff = staff;
        UserId = id;
        CanEditWithElevatedPrivilege = await authorization.Succeeded(User, Policies.SuperUserAdministrator);

        await PopulateRoleSettingsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var rolesDictionary = RoleSettings.ToDictionary(setting => setting.Name, setting => setting.IsSelected);
        var result = await staffService.UpdateRolesAsync(UserId, rolesDictionary);

        if (result.Succeeded)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "User roles successfully updated.");
            return RedirectToPage("Details", new { id = UserId });
        }

        foreach (var err in result.Errors)
            ModelState.AddModelError(string.Empty, string.Concat(err.Code, ": ", err.Description));

        var staff = await staffService.FindAsync(UserId);
        if (staff is null) return BadRequest();

        DisplayStaff = staff;

        return Page();
    }

    private async Task PopulateRoleSettingsAsync()
    {
        var roles = await staffService.GetRolesAsync(DisplayStaff.Id);

        RoleSettings.AddRange(AppRole.AllRoles.Select(pair => new RoleSetting
        {
            Name = pair.Key,
            DisplayName = pair.Value.DisplayName,
            Description = pair.Value.Description,
            IsSelected = roles.Contains(pair.Key),
        }));
    }

    public class RoleSetting
    {
        public string Name { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public bool IsSelected { get; init; }
    }
}
