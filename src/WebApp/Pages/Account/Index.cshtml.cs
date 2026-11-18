using Cts.AppServices.AuthorizationPolicies;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;

namespace Cts.WebApp.Pages.Account;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class AccountIndexModel([FromServices] IStaffService staffService) : PageModel
{
    public StaffViewDto DisplayStaff { get; private set; } = null!;
    public UserPreferences Preferences { get; private set; } = null!;
    public string? OfficeName => DisplayStaff.OfficeName;
    public IReadOnlyList<AppRole> Roles { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync([FromServices] IStaffService staffService)
    {
        DisplayStaff = await staffService.GetCurrentUserAsync();
        Roles = await staffService.GetAppRolesAsync(DisplayStaff.Id);
        Preferences = await staffService.GetPreferencesAsync();
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(ThemePreference theme)
    {
        await staffService.UpdateThemePreferenceAsync(theme);
        return RedirectToPage();
    }
}
