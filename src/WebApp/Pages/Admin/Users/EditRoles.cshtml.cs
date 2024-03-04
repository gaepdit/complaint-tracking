using Cts.AppServices.Permissions;
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
    public List<RoleSetting> RoleSettings { get; set; } = new();

    public StaffViewDto DisplayStaff { get; private set; } = default!;
    public string? OfficeName => DisplayStaff.Office?.Name;
    public bool CanEditDivisionManager { get; private set; }

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null) return RedirectToPage("Index");
        var staff = await staffService.FindAsync(id);
        if (staff is null) return NotFound();

        DisplayStaff = staff;
        UserId = id;
        CanEditDivisionManager = (await authorization.AuthorizeAsync(User, Policies.DivisionManager)).Succeeded;

        await PopulateRoleSettingsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        CanEditDivisionManager = (await authorization.AuthorizeAsync(User, Policies.DivisionManager)).Succeeded;

        var roleDictionary = CanEditDivisionManager
            ? RoleSettings.ToDictionary(r => r.Name, r => r.IsSelected)
            : RoleSettings.Where(e => e.Name != RoleName.DivisionManager)
                .ToDictionary(r => r.Name, r => r.IsSelected);

        var result = await staffService.UpdateRolesAsync(UserId, roleDictionary);

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

        RoleSettings.AddRange(AppRole.AllRoles.Select(r => new RoleSetting
        {
            Name = r.Key,
            DisplayName = r.Value.DisplayName,
            Description = r.Value.Description,
            IsSelected = roles.Contains(r.Key),
        }));
    }

    public class RoleSetting
    {
        public string Name { get; init; } = default!;
        public string DisplayName { get; init; } = default!;
        public string Description { get; init; } = default!;
        public bool IsSelected { get; init; }
    }
}
