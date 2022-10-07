using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAppRoot.Domain.Identity;
using MyAppRoot.WebApp.Platform.Local;

namespace MyAppRoot.WebApp.Pages.Account;

[AllowAnonymous]
public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IWebHostEnvironment _environment;

    public LogoutModel(SignInManager<ApplicationUser> signInManager, IWebHostEnvironment environment)
    {
        _signInManager = signInManager;
        _environment = environment;
    }

    public Task<IActionResult> OnGetAsync() => LogOutAndRedirectToIndex();

    public Task<IActionResult> OnPostAsync() => LogOutAndRedirectToIndex();

    private async Task<IActionResult> LogOutAndRedirectToIndex()
    {
        if (!_environment.IsLocalEnv())
        {
#pragma warning disable 618
            return SignOut(IdentityConstants.ApplicationScheme, IdentityConstants.ExternalScheme,
                AzureADDefaults.OpenIdScheme);
#pragma warning restore 618
        }

        // If "test" users is enabled, sign out locally and redirect to home page.
        await _signInManager.SignOutAsync();
        return RedirectToPage("/Index");
    }
}
