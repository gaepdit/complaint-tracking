using Cts.Domain.Identity;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
    public Task<IActionResult> OnGetAsync() => LogOutAndRedirectToIndex();

    public Task<IActionResult> OnPostAsync() => LogOutAndRedirectToIndex();

    private async Task<IActionResult> LogOutAndRedirectToIndex()
    {
        // If Azure AD is enabled, sign out all authentication schemes.
        if (AppSettings.DevSettings.UseAzureAd)
            return SignOut(new AuthenticationProperties { RedirectUri = "/Index" },
                IdentityConstants.ApplicationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);

        // If a local user is enabled instead, sign out locally and redirect to home page.
        await signInManager.SignOutAsync();
        return RedirectToPage("/Index");
    }
}
