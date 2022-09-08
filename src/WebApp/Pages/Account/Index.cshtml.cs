using Cts.AppServices.StaffServices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.Local;
using Cts.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        [FromServices] IStaffAppService staffService,
        [FromServices] SignInManager<ApplicationUser> signInManager,
        [FromServices] IWebHostEnvironment environment)
    {
        var currentUser = await userService.GetCurrentUserAsync();

        if (currentUser is null)
        {
            if (!environment.IsLocalEnv())
            {
#pragma warning disable 618
                return SignOut(IdentityConstants.ApplicationScheme,
                    IdentityConstants.ExternalScheme, AzureADDefaults.OpenIdScheme);
#pragma warning restore 618
            }

            // If "test" users is enabled, sign out locally and redirect to home page.
            await signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }

        var staff = await staffService.FindAsync(currentUser.IdGuid);
        if (staff == null) return NotFound("ID not found.");

        DisplayStaff = staff;
        Roles = await staffService.GetAppRolesAsync(DisplayStaff.Id);
        Message = TempData.GetDisplayMessage();

        return Page();
    }
}
