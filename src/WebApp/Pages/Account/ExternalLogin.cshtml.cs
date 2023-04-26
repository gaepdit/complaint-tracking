using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class ExternalLoginModel : PageModel
{
    // Constructor
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IStaffAppService _staffService;
    private readonly ILogger<ExternalLoginModel> _logger;

    public ExternalLoginModel(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IStaffAppService staffService,
        ILogger<ExternalLoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _staffService = staffService;
        _logger = logger;
    }

    // Properties
    public ApplicationUser? DisplayFailedUser { get; private set; }
    public string ReturnUrl { get; private set; } = "/";

    // Methods

    // Don't call the page directly
    public IActionResult OnGet() => RedirectToPage("./Login");

    // This Post method is called from the Login page
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? "/";

        // Use AzureAD authentication if enabled; otherwise, sign in as local user.
        if (!ApplicationSettings.DevSettings.UseAzureAd) return await SignInAsLocalUser();

        // Request a redirect to the external login provider.
        const string provider = OpenIdConnectDefaults.AuthenticationScheme;
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    private async Task<IActionResult> SignInAsLocalUser()
    {
        _logger.LogInformation(
            "Local user signin attempted with settings {LocalUserIsAuthenticated} and {LocalUserIsAdmin}",
            ApplicationSettings.DevSettings.LocalUserIsAuthenticated,
            ApplicationSettings.DevSettings.LocalUserIsAdmin);
        if (!ApplicationSettings.DevSettings.LocalUserIsAuthenticated) return Forbid();

        var staff = ApplicationSettings.DevSettings.LocalUserIsAdmin
            ? (await _staffService.GetListAsync(new StaffSearchDto { Name = "Admin" })).First()
            : (await _staffService.GetListAsync(new StaffSearchDto { Name = "General" })).First();

        var user = await _userManager.FindByIdAsync(staff.Id);
        _logger.LogInformation("Local user {StaffName} with ID {StaffId} signed in", staff.Name, staff.Id);

        await _signInManager.SignInAsync(user!, false);
        return LocalRedirect(ReturnUrl);
    }

    // This callback method is called by the external login provider.
    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        ReturnUrl = returnUrl ?? "/";

        // Handle errors returned from the external provider.
        if (remoteError is not null)
            return RedirectToLoginPageWithError($"Error from work account provider: {remoteError}");

        // Get information about the user from the external provider.
        var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo?.Principal is null)
            return RedirectToLoginPageWithError("Error loading work account information.");

        var preferredUserName = externalLoginInfo.Principal.FindFirstValue(ClaimConstants.PreferredUserName);
        if (preferredUserName is null)
            return RedirectToLoginPageWithError("Error loading detailed work account information.");

        // Determine if a user account already exists.
        var user = await _userManager.FindByNameAsync(preferredUserName);

        // If the user does not have an account yet, then create one and sign in.
        if (user is null)
            return await CreateUserAndSignInAsync(externalLoginInfo);

        // If user has been marked as inactive, don't sign in.
        if (!user.Active)
        {
            _logger.LogWarning("Inactive user {UserName} attempted signin", preferredUserName);
            return RedirectToPage("./Unavailable");
        }

        // Sign in the user locally with the external provider key.
        var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey, true, true);

        if (signInResult.Succeeded)
            return await RefreshUserInfoAndSignInAsync(user, externalLoginInfo);

        if (signInResult.IsLockedOut || signInResult.IsNotAllowed || signInResult.RequiresTwoFactor)
            return RedirectToPage("./Unavailable");

        // If ExternalLoginInfo successfully returned from external provider, and the user exists, but
        // ExternalLoginSignInAsync failed, then add the external provider info to the user and sign in.
        return await AddLoginProviderAndSignInAsync(user, externalLoginInfo);
    }

    // Redirect to Login page with error message.
    private IActionResult RedirectToLoginPageWithError(string message)
    {
        _logger.LogWarning("External login error: {Message}", message);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Danger, message);
        return RedirectToPage("./Login", new { ReturnUrl });
    }

    // Create a new user account and sign in.
    private async Task<IActionResult> CreateUserAndSignInAsync(ExternalLoginInfo info)
    {
        var user = new ApplicationUser
        {
            UserName = info.Principal.FindFirstValue(ClaimConstants.PreferredUserName),
            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
            GivenName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "",
            FamilyName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "",
            AzureAdObjectId = info.Principal.FindFirstValue(ClaimConstants.ObjectId),
        };

        // Create the user in the backing store.
        var createUserResult = await _userManager.CreateAsync(user);
        if (!createUserResult.Succeeded)
        {
            _logger.LogWarning("Failed to create new user {UserName}", user.UserName);
            return FailedLogin(createUserResult, user);
        }

        _logger.LogInformation("Created new user {UserName}", user.UserName);

        // Add new user to application Roles if seeded in app settings or local admin user setting is enabled.
        var seedUsers = _configuration.GetSection("SeedAdminUsers").Get<string[]>();
        if (ApplicationSettings.DevSettings.LocalUserIsAdmin ||
            (seedUsers != null && seedUsers.Contains(user.Email, StringComparer.InvariantCultureIgnoreCase)))
        {
            _logger.LogInformation("Seeding roles for new user {UserName}", user.UserName);
            foreach (var role in AppRole.AllRoles) await _userManager.AddToRoleAsync(user, role.Key);
        }

        // Add the external provider info to the user and sign in.
        ReturnUrl = "/Account/Index";
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            "Your account has successfully been created. Select “Edit Profile” to update your info.");
        return await AddLoginProviderAndSignInAsync(user, info);
    }

    // Update local store with from external provider. 
    private async Task<IActionResult> RefreshUserInfoAndSignInAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        _logger.LogInformation("Existing user {UserName} logged in with {LoginProvider} provider",
            user.UserName, info.LoginProvider);
        user.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
        user.GivenName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "";
        user.FamilyName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "";
        await _userManager.UpdateAsync(user);
        await _signInManager.RefreshSignInAsync(user);
        return LocalRedirect(ReturnUrl);
    }

    // Add external login provider to user account and sign in user.
    private async Task<IActionResult> AddLoginProviderAndSignInAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        var addLoginResult = await _userManager.AddLoginAsync(user, info);

        if (!addLoginResult.Succeeded)
        {
            _logger.LogWarning("Failed to add login provider {LoginProvider} for user {UserName}",
                info.LoginProvider, user.UserName);
            return FailedLogin(addLoginResult, user);
        }

        _logger.LogInformation("Login provider {LoginProvider} added for user {UserName}",
            info.LoginProvider, user.UserName);

        // Include the access token in the properties.
        var props = new AuthenticationProperties();
        if (info.AuthenticationTokens is not null) props.StoreTokens(info.AuthenticationTokens);
        props.IsPersistent = true;

        await _signInManager.SignInAsync(user, props, info.LoginProvider);
        return LocalRedirect(ReturnUrl);
    }

    // Add error info and return this Page.
    private IActionResult FailedLogin(IdentityResult result, ApplicationUser user)
    {
        DisplayFailedUser = user;
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        return Page();
    }
}
