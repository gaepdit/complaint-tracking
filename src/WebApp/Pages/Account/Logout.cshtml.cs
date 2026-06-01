using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
    public Task<SignOutResult> OnGetAsync() => SignOut();
    public Task<SignOutResult> OnPostAsync(string? returnUrl = null) => SignOut(returnUrl);

    private async Task<SignOutResult> SignOut(string? returnUrl = null)
    {
        var authenticationProperties = new AuthenticationProperties { RedirectUri = returnUrl ?? "../" };
        await signInManager.SignOutAsync();
        return SignOut(authenticationProperties);
    }
}
