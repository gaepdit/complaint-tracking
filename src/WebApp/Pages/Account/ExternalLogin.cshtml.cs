using AutoMapper;
using Cts.AppServices.StaffServices;
using Cts.Domain.Entities;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.Local;
using Cts.WebApp.Platform.RazorHelpers;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class ExternalLogin : PageModel
{
    [BindProperty]
    public ApplicationUser? DisplayFailedUser { get; private set; }

    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    private readonly IStaffAppService _staffService;


    public ExternalLogin(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IWebHostEnvironment environment,
        IStaffAppService staffService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _environment = environment;
        _staffService = staffService;
    }

    // Don't call the page directly
    public IActionResult OnGet() => RedirectToPage("./Login");

    // This Post method is called by the Login page
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        // If "test" users is enabled, create user information and sign in locally.
        if (_environment.IsLocalEnv()) return await SignInAsLocalUser();

        // Request a redirect to the external login provider.
#pragma warning disable 618
        const string provider = AzureADDefaults.AuthenticationScheme;
#pragma warning restore 618
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);

        async Task<IActionResult> SignInAsLocalUser()
        {
            if (!ApplicationSettings.LocalDevSettings.AuthenticatedUser) return Forbid();

            var staff = ApplicationSettings.LocalDevSettings.AuthenticatedUserIsAdmin
                ? (await _staffService.GetListAsync(new StaffSearchDto { Name = "Admin" })).First()
                : (await _staffService.GetListAsync(new StaffSearchDto { Name = "General" })).First();

            var user = await _userManager.FindByIdAsync(staff.Id.ToString());

            await _signInManager.SignInAsync(user, false);
            return LocalRedirect(returnUrl ?? "/");
        }
    }

    // This method is called by the external login provider.
    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        if (remoteError is not null)
            return RedirectToLoginPageWithError(returnUrl, $"Error from work account provider: {remoteError}");

        // Get information about the user from the external provider.
        var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();

        if (externalLoginInfo is null
            || !externalLoginInfo.Principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier)
            || !externalLoginInfo.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            return RedirectToLoginPageWithError(returnUrl, "Error loading work account information.");

        // Sign in the user with the external provider.
        var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey, true);

        if (signInResult.Succeeded) return LocalRedirect(returnUrl ?? "/");

        if (signInResult.IsLockedOut || signInResult.IsNotAllowed || signInResult.RequiresTwoFactor)
            return RedirectToPage("./Unavailable");

        // If ExternalLoginInfo successfully returned from external provider, but ExternalLoginSignInAsync
        // failed, local account may need to be configured. Start by checking if an account exists.
        var userEmail = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        var existingUser = await _userManager.FindByNameAsync(userEmail);

        // If user account exists, add the external provider info to the user and sign in.
        if (existingUser is not null) return await AddProviderAndSignInUserAsync(existingUser, externalLoginInfo);

        // If the user does not have an account, then create one and sign in.
        var newUser = new ApplicationUser
        {
            Email = userEmail,
            LastName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
            FirstName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname),
        };

        // Create the user in the backing store.
        var createUserResult = await _userManager.CreateAsync(newUser);
        if (!createUserResult.Succeeded) return FailedLogin(createUserResult, newUser);

        // Add new user to application Roles if seeded in app settings.
        var seedUsers = _configuration.GetSection("SeedAdminUsers").Get<string[]>().AsEnumerable();
        if (seedUsers.Contains(newUser.Email, StringComparer.InvariantCultureIgnoreCase))
            foreach (var role in AppRole.AllRoles)
                await _userManager.AddToRoleAsync(newUser, role.Key);

        // Add the external provider info to the user and sign in.
        return await AddProviderAndSignInUserAsync(newUser, externalLoginInfo);

        // Local function: Add external provider info to user account, sign in user, and redirect
        // to original requested URL.
        async Task<IActionResult> AddProviderAndSignInUserAsync(ApplicationUser user, ExternalLoginInfo loginInfo)
        {
            var addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);
            if (!addLoginResult.Succeeded) return FailedLogin(addLoginResult, user);

            // Include the access token in the properties.
            var props = new AuthenticationProperties();
            props.StoreTokens(loginInfo.AuthenticationTokens);
            props.IsPersistent = true;

            await _signInManager.SignInAsync(user, true);
            return LocalRedirect(returnUrl ?? "/");
        }

        // Local function: Add errors from failed login and return this Page.
        IActionResult FailedLogin(IdentityResult result, ApplicationUser user)
        {
            DisplayFailedUser = user;
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            return Page();
        }

        // Local function: Redirect to Login page with error message.
        IActionResult RedirectToLoginPageWithError(string? rUrl, string message)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Danger, message);
            return RedirectToPage("./Login", new { ReturnUrl = rUrl });
        }
    }
}
