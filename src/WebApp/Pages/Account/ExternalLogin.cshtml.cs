using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class ExternalLoginModel(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    IStaffService staffService,
    ILogger<ExternalLoginModel> logger)
    : PageModel
{
    public ApplicationUser? DisplayFailedUser { get; private set; }
    public string? ReturnUrl { get; private set; }

    // Don't call this page directly
    public RedirectToPageResult OnGet() => RedirectToPage("./Login");

    // This Post method is called from the Login page
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;

        // Use AzureAD authentication if enabled; otherwise, sign in as local user.
        if (!AppSettings.DevSettings.UseAzureAd) return await SignInAsLocalUser();

        // Request a redirect to the external login provider.
        const string provider = OpenIdConnectDefaults.AuthenticationScheme;
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    private async Task<IActionResult> SignInAsLocalUser()
    {
        logger.LogInformation(
            "Local user signin attempted with settings {LocalUserIsAuthenticated}, {LocalUserIsAdmin}, and {LocalUserIsStaff}",
            AppSettings.DevSettings.LocalUserIsAuthenticated.ToString(),
            AppSettings.DevSettings.LocalUserIsAdmin.ToString(),
            AppSettings.DevSettings.LocalUserIsStaff.ToString());
        if (!AppSettings.DevSettings.LocalUserIsAuthenticated) return Forbid();

        StaffSearchDto search;

        if (AppSettings.DevSettings.LocalUserIsAdmin)
            search = new StaffSearchDto(SortBy.NameAsc, "Admin", null, null, null, null);
        else if (AppSettings.DevSettings.LocalUserIsStaff)
            search = new StaffSearchDto(SortBy.NameAsc, "General", null, null, null, null);
        else
            search = new StaffSearchDto(SortBy.NameAsc, "Limited", null, null, null, null);

        var staffId = (await staffService.GetListAsync(search))[0].Id;

        var user = await userManager.FindByIdAsync(staffId);
        logger.LogInformation("Local user with ID {StaffId} signed in", staffId);

        await signInManager.SignInAsync(user!, false);
        return LocalRedirectOrHome();
    }

    // This callback method is called by the external login provider.
    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        ReturnUrl = returnUrl;

        // Handle errors returned from the external provider.
        if (remoteError is not null)
            return RedirectToLoginPageWithError($"Error from work account provider: {remoteError}");

        // Get information about the user from the external provider.
        var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo?.Principal is null)
            return RedirectToLoginPageWithError("Error loading work account information.");

        var preferredUserName = externalLoginInfo.Principal.FindFirstValue(ClaimConstants.PreferredUserName);
        if (preferredUserName is null)
            return RedirectToLoginPageWithError("Error loading detailed work account information.");

        if (!preferredUserName.IsValidEmailDomain())
        {
            logger.LogWarning("User {UserName} with invalid email domain attempted signin", preferredUserName);
            return RedirectToPage("./Unavailable");
        }

        // Determine if a user account already exists.
        var user = await userManager.FindByNameAsync(preferredUserName);

        // If the user does not have a local account yet, then create one and sign in.
        if (user is null)
            return await CreateUserAndSignInAsync(externalLoginInfo);

        // If user has been marked as inactive, don't sign in.
        if (!user.Active)
        {
            logger.LogWarning("Inactive user {UserName} attempted signin", preferredUserName);
            return RedirectToPage("./Unavailable");
        }

        // Try to sign in the user locally with the external provider key.
        var signInResult = await signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey, true, true);

        if (signInResult.IsLockedOut || signInResult.IsNotAllowed || signInResult.RequiresTwoFactor)
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("./Unavailable");
        }

        if (signInResult.Succeeded)
            return await RefreshUserInfoAndSignInAsync(user, externalLoginInfo);

        // If ExternalLoginInfo successfully returned from external provider, and the user exists, but
        // ExternalLoginSignInAsync failed, then add the external provider info to the user and sign in.
        // (Implied `signInResult.Succeeded == false`.)
        return await AddLoginProviderAndSignInAsync(user, externalLoginInfo);
    }

    // Redirect to Login page with error message.
    private RedirectToPageResult RedirectToLoginPageWithError(string message)
    {
        logger.LogWarning("External login error: {Message}", message);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Danger, message);
        return RedirectToPage("./Login", new { ReturnUrl });
    }

    // Create a new user account and sign in.
    private async Task<IActionResult> CreateUserAndSignInAsync(ExternalLoginInfo info)
    {
        var user = new ApplicationUser
        {
            UserName = info.Principal.FindFirstValue(ClaimConstants.PreferredUserName),
            Email = info.Principal.FindFirstValue(ClaimTypes.Email) ??
                    info.Principal.FindFirstValue(ClaimConstants.PreferredUserName),
            GivenName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "",
            FamilyName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "",
            ObjectIdentifier = info.Principal.FindFirstValue(ClaimConstants.ObjectId),
        };

        // Create the user in the backing store.
        var createUserResult = await userManager.CreateAsync(user);
        if (!createUserResult.Succeeded)
        {
            logger.LogWarning("Failed to create new user {UserName}", user.UserName);
            return await FailedLoginAsync(createUserResult, user);
        }

        logger.LogInformation("Created new user {UserName} with object ID {ObjectId}", user.UserName,
            user.ObjectIdentifier);

        // Add new user to application Roles if seeded in app settings or local admin user setting is enabled.
        var seedAdminUsers = configuration.GetSection("SeedAdminUsers").Get<string[]>();
        if (AppSettings.DevSettings.LocalUserIsStaff)
        {
            logger.LogInformation("Seeding staff role for new user {UserName}", user.UserName);
            await userManager.AddToRoleAsync(user, RoleName.Staff);
        }

        if (AppSettings.DevSettings.LocalUserIsAdmin ||
            (seedAdminUsers != null && seedAdminUsers.Contains(user.Email, StringComparer.InvariantCultureIgnoreCase)))
        {
            logger.LogInformation("Seeding all roles for new user {UserName}", user.UserName);
            foreach (var role in AppRole.AllRoles) await userManager.AddToRoleAsync(user, role.Key);
        }

        // Add the external provider info to the user and sign in.
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            "Your account has successfully been created. Select “Edit Profile” to update your info.");
        return await AddLoginProviderAndSignInAsync(user, info, newUser: true);
    }

    // Update local store with from external provider. 
    private async Task<IActionResult> RefreshUserInfoAndSignInAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        logger.LogInformation("Existing user {UserName} logged in with {LoginProvider} provider",
            user.UserName, info.LoginProvider);
        user.Email = info.Principal.FindFirstValue(ClaimTypes.Email) ??
                     info.Principal.FindFirstValue(ClaimConstants.PreferredUserName);
        user.GivenName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? user.GivenName;
        user.FamilyName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? user.FamilyName;
        await userManager.UpdateAsync(user);
        await signInManager.RefreshSignInAsync(user);
        return LocalRedirectOrHome();
    }

    // Add external login provider to user account and sign in user.
    private async Task<IActionResult> AddLoginProviderAndSignInAsync(
        ApplicationUser user, ExternalLoginInfo info, bool newUser = false)
    {
        var addLoginResult = await userManager.AddLoginAsync(user, info);

        if (!addLoginResult.Succeeded)
        {
            logger.LogWarning("Failed to add login provider {LoginProvider} for user {UserName}",
                info.LoginProvider, user.UserName);
            return await FailedLoginAsync(addLoginResult, user);
        }

        if (user.ObjectIdentifier == null)
        {
            user.ObjectIdentifier = info.Principal.FindFirstValue(ClaimConstants.ObjectId);
            await userManager.UpdateAsync(user);
        }

        logger.LogInformation("Login provider {LoginProvider} added for user {UserName} with object ID {ObjectId}",
            info.LoginProvider, user.UserName, user.ObjectIdentifier);

        // Include the access token in the properties.
        var props = new AuthenticationProperties();
        if (info.AuthenticationTokens is not null) props.StoreTokens(info.AuthenticationTokens);
        props.IsPersistent = true;

        await signInManager.SignInAsync(user, props, info.LoginProvider);
        // If new user, redirect to Account page
        return newUser ? RedirectToPage("./Index") : LocalRedirectOrHome();
    }

    // Add error info and return this Page.
    private async Task<PageResult> FailedLoginAsync(IdentityResult result, ApplicationUser user)
    {
        DisplayFailedUser = user;
        await signInManager.SignOutAsync();
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        return Page();
    }

    private IActionResult LocalRedirectOrHome()
    {
        if (ReturnUrl is null) return RedirectToPage("/Staff/Index");
        return LocalRedirect(ReturnUrl);
    }
}
