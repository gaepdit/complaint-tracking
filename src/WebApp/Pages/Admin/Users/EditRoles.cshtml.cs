using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Users;

[Authorize(Roles = AppRole.UserAdmin)]
public class EditRolesModel : PageModel
{
    private readonly IStaffAppService _staffService;
    public EditRolesModel(IStaffAppService staffService) => _staffService = staffService;

    public StaffViewDto DisplayStaff { get; private set; } = default!;
    public string? OfficeName => DisplayStaff.Office?.Name;

    [BindProperty]
    public string UserId { get; set; } = string.Empty;

    [BindProperty]
    public List<RoleSetting> RoleSettings { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null) return RedirectToPage("Index");
        var staff = await _staffService.FindAsync(id);
        if (staff is null) return NotFound();

        DisplayStaff = staff;
        UserId = id;

        await PopulateRoleSettingsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await _staffService.UpdateRolesAsync(UserId,
            RoleSettings.ToDictionary(r => r.Name, r => r.IsSelected));

        if (result.Succeeded)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "User roles successfully updated.");
            return RedirectToPage("Details", new { id = UserId });
        }

        foreach (var err in result.Errors)
            ModelState.AddModelError(string.Empty, string.Concat(err.Code, ": ", err.Description));

        var staff = await _staffService.FindAsync(UserId);
        if (staff is null) return BadRequest();

        DisplayStaff = staff;

        return Page();
    }

    private async Task PopulateRoleSettingsAsync()
    {
        var roles = await _staffService.GetRolesAsync(DisplayStaff.Id);

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
