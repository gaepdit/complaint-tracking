using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
    public Task<SignOutResult> OnGetAsync() => SignOut();
    public Task<SignOutResult> OnPostAsync() => SignOut();

    private async Task<SignOutResult> SignOut()
    {
        var authenticationProperties = new AuthenticationProperties { RedirectUri = "../" };
        await signInManager.SignOutAsync();
        return SignOut(authenticationProperties);
    }
}
