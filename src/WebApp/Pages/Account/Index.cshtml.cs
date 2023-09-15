using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Account;

[Authorize(Policy = nameof(Policies.LoggedInUser))]
public class IndexModel : PageModel
{
    public StaffViewDto DisplayStaff { get; private set; } = default!;
    public string? OfficeName => DisplayStaff.Office?.Name;
    public IList<AppRole> Roles { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync([FromServices] IStaffService staffService)
    {
        DisplayStaff = await staffService.GetCurrentUserAsync();
        Roles = await staffService.GetAppRolesAsync(DisplayStaff.Id);
        return Page();
    }
}
