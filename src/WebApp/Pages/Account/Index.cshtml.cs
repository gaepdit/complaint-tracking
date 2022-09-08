using Cts.AppServices.StaffServices;
using Cts.AppServices.UserServices;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Account;

[Authorize]
public class IndexModel : PageModel
{
    public StaffViewDto DisplayStaff { get; private set; } = default!;
    public string? OfficeName => DisplayStaff.Office?.Name;
    public IList<AppRole> Roles { get; private set; } = default!;
    public DisplayMessage? Message { get; private set; }

    public async Task<IActionResult> OnGetAsync(
        [FromServices] IUserService userService,
        [FromServices] IStaffAppService staffService)
    {
        var currentUser = await userService.GetCurrentUserAsync();
        if (currentUser is null) return Forbid();

        var staff = await staffService.FindAsync(currentUser.IdAsGuid);
        if (staff == null) return Forbid();

        DisplayStaff = staff;
        Roles = await staffService.GetAppRolesAsync(DisplayStaff.Id);
        Message = TempData.GetDisplayMessage();

        return Page();
    }
}
