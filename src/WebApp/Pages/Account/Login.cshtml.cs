using Cts.AppServices.IdentityServices;
using Cts.AppServices.IdentityServices.Claims;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class LoginModel(
    SignInManager<ApplicationUser> signInManager,
    IIdentityManager identityManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    ILogger<LoginModel> logger
) : PageModel
{
    public string? ReturnUrl { get; private set; }

    public bool DisplayFailedLogin { get; private set; }

    public IActionResult OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        if (User.Identity is not { IsAuthenticated: true }) return Page();
        return User.IsActive() ? LocalRedirectOrHome() : RedirectToPage("Logout");
    }

    public async Task<IActionResult> OnPostTestUserAsync(string? returnUrl = null)
    {
        if (AppSettings.DevSettings.UseExternalAuthentication) return BadRequest();
        if (!AppSettings.DevSettings.LocalUserIsAuthenticated) return Forbid();

        ReturnUrl = returnUrl;

        var user = await userManager.FindByIdAsync("00000000-0000-0000-0000-000000000001");
        logger.LogInformation("Local user with ID {StaffId} signed in", user!.Id);

        foreach (var pair in AppRole.AllRoles)
            await userManager.RemoveFromRoleAsync(user, pair.Value.Name);
        foreach (var role in AppSettings.DevSettings.LocalUserRoles)
            await userManager.AddToRoleAsync(user, role);

        await signInManager.SignInAsync(user, false);
        return LocalRedirectOrHome();
    }

    public IActionResult OnPostAsync(string scheme)
    {
        if (!AppSettings.DevSettings.UseExternalAuthentication) return BadRequest();
        if (User.Identity is { IsAuthenticated: true }) return RedirectToPage("Logout");
        if (!configuration.ValidateLoginProvider(scheme))
            throw new ArgumentException("Invalid scheme", nameof(scheme));

        // Request a redirect to the external login provider.
        var redirectUrl = Url.Page("Login", pageHandler: "Callback", values: new { ReturnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(scheme, redirectUrl);
        return Challenge(properties, scheme);
    }

    // The callback method is called by the external login provider.
    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        ReturnUrl = returnUrl;
        if (remoteError is not null)
            return LoginPageWithError($"Error from work account provider: {remoteError}");
        var result = await identityManager.LogInUsingExternalProviderAsync();
        return result.Succeeded ? LocalRedirectOrHome() : await FailedLoginAsync(result);
    }

    private RedirectToPageResult LoginPageWithError(string message)
    {
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Danger, message);
        return RedirectToPage("Login", new { ReturnUrl });
    }

    private async Task<PageResult> FailedLoginAsync(IdentityResult result)
    {
        await signInManager.SignOutAsync();
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
        DisplayFailedLogin = true;
        return Page();
    }

    private IActionResult LocalRedirectOrHome() =>
        ReturnUrl is null ? RedirectToPage("/Staff/Index") : LocalRedirect(ReturnUrl);
}
