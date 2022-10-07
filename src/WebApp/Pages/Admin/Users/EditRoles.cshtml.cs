using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.Domain.Identity;
using MyAppRoot.WebApp.Models;
using MyAppRoot.WebApp.Platform.RazorHelpers;

namespace MyAppRoot.WebApp.Pages.Admin.Users;

[Authorize(Roles = AppRole.UserAdmin)]
public class EditRolesModel : PageModel
{
    private readonly IStaffAppService _staffService;
    public EditRolesModel(IStaffAppService staffService) => _staffService = staffService;

    public StaffViewDto DisplayStaff { get; private set; } = default!;
    public string? OfficeName => DisplayStaff.Office?.Name;

    [BindProperty]
    public Guid UserId { get; set; }

    [BindProperty]
    public List<RoleSetting> RoleSettings { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();
        var staff = await _staffService.FindAsync(id.Value);
        if (staff == null) return NotFound("ID not found.");

        DisplayStaff = staff;
        UserId = id.Value;

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
        if (staff == null) return BadRequest();

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
        public string Name { get; init; } = null!;
        public string DisplayName { get; init; } = null!;
        public string Description { get; init; } = null!;
        public bool IsSelected { get; init; }
    }
}
