using Cts.AppServices.IdentityServices.Claims;
using Cts.Domain.Identity;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
    public Task<SignOutResult> OnGetAsync() => LogOutAndRedirectToIndex();
    public Task<SignOutResult> OnPostAsync() => LogOutAndRedirectToIndex();

    private async Task<SignOutResult> LogOutAndRedirectToIndex()
    {
        var authenticationProperties = new AuthenticationProperties { RedirectUri = "/Index" };

        if (!AppSettings.DevSettings.UseExternalAuthentication)
        {
            await signInManager.SignOutAsync();
            return SignOut(authenticationProperties);
        }

        List<string> authenticationSchemes = [CookieAuthenticationDefaults.AuthenticationScheme,];

        var userAuthenticationScheme = User.GetAuthenticationMethod();
        if (userAuthenticationScheme != null)
            authenticationSchemes.AddRange([IdentityConstants.ApplicationScheme, userAuthenticationScheme]);

        return SignOut(authenticationProperties, authenticationSchemes.ToArray());
    }
}
