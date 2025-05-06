using Cts.AppServices.AuthorizationPolicies;
using Cts.AppServices.IdentityServices;
using Cts.AppServices.IdentityServices.Claims;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Cts.WebApp.Platform.Settings;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

namespace Cts.WebApp.Pages.Account;

[AllowAnonymous]
public class LoginModel(
    IAuthorizationService authorization,
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    ILogger<LoginModel> logger
) : PageModel
{
    [BindProperty]
    public string? ReturnUrl { get; private set; }

    public ApplicationUser? DisplayFailedUser { get; private set; }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            if (!await authorization.Succeeded(User, Policies.ActiveUser)) return RedirectToPage("Logout");
            return string.IsNullOrEmpty(returnUrl) ? RedirectToPage("/Index") : LocalRedirect(returnUrl);
        }

        ReturnUrl = returnUrl;
        return Page();
    }

    public async Task<IActionResult> OnPostTestUserAsync() =>
        AppSettings.DevSettings.UseExternalAuthentication ? BadRequest() : await SignInAsLocalUser();

    public IActionResult OnPostAsync(string scheme)
    {
        if (!AppSettings.DevSettings.UseExternalAuthentication) return BadRequest();
        if (User.Identity is { IsAuthenticated: true }) return RedirectToPage("Logout");

        if (scheme != IdentityProviders.OktaScheme && scheme != IdentityProviders.EntraIdScheme)
            throw new ArgumentException("Invalid scheme", nameof(scheme));

        // Request a redirect to the external login provider.
        var redirectUrl = Url.Page("Login", pageHandler: "Callback", values: new { ReturnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(scheme, redirectUrl);
        return Challenge(properties, scheme);
    }

    private async Task<IActionResult> SignInAsLocalUser()
    {
        logger.LogInformation("Local user signin attempted with setting {LocalUserIsAuthenticated}",
            AppSettings.DevSettings.LocalUserIsAuthenticated.ToString());

        if (!AppSettings.DevSettings.LocalUserIsAuthenticated) return Forbid();

        var user = await userManager.FindByIdAsync("00000000-0000-0000-0000-000000000001");
        logger.LogInformation("Local user with ID {StaffId} signed in", user!.Id);

        foreach (var pair in AppRole.AllRoles) await userManager.RemoveFromRoleAsync(user, pair.Value.Name);
        foreach (var role in AppSettings.DevSettings.LocalUserRoles) await userManager.AddToRoleAsync(user, role);

        await signInManager.SignInAsync(user, false);
        return LocalRedirectOrHome();
    }

    // The callback method is called by the external login provider.
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

        var loginProvider = externalLoginInfo.LoginProvider;

        var identityProviderId = externalLoginInfo.Principal.GetIdentityProviderId();
        var userEmail = externalLoginInfo.Principal.GetEmail();
        if (identityProviderId is null || userEmail is null)
            return RedirectToLoginPageWithError("Error loading detailed work account information.");

        if (!configuration.ValidateIdentityProvider(loginProvider, identityProviderId))
        {
            logger.LogWarning(
                "User from disallowed provider {LoginProvider} with ID {IdentityProviderId} attempted signin",
                loginProvider, identityProviderId);
            return RedirectToLoginPageWithError(
                $"Invalid login provider '{loginProvider}' with ID '{identityProviderId}'.");
        }

        logger.LogInformation("User with ID {SubjectId} in provider {LoginProvider} successfully authenticated",
            externalLoginInfo.Principal.GetSubjectId(loginProvider), loginProvider);

        // Determine if a user account already exists with the Object ID.
        // If not, then determine if a user account already exists with the given username.
        var user = AppSettings.DevSettings.UseInMemoryData
            ? await userManager.FindByNameAsync(userEmail)
            : await userManager.Users.SingleOrDefaultAsync(u =>
                  loginProvider == IdentityProviders.OktaScheme &&
                  u.OktaSubjectId == externalLoginInfo.Principal.GetOktaSubjectId(loginProvider) ||
                  loginProvider == IdentityProviders.EntraIdScheme &&
                  u.EntraIdSubjectId == externalLoginInfo.Principal.GetEntraSubjectId(loginProvider)) ??
              await userManager.FindByNameAsync(userEmail);

        // If the user does not have a local account yet, then create one and sign in.
        if (user is null)
            return await CreateUserAndSignInAsync(externalLoginInfo);

        // If user has been marked as inactive, don't sign in.
        if (!user.Active)
        {
            logger.LogWarning("Inactive user with object ID {ObjectId} attempted signin", user.EntraIdSubjectId);
            return RedirectToPage("./Unavailable");
        }

        // Try to sign in the user locally with the external provider key.
        var signInResult = await signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey, isPersistent: true, bypassTwoFactor: true);

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
            UserName = info.Principal.GetDisplayName(),
            Email = info.Principal.GetEmail(),
            GivenName = info.Principal.GetGivenName(),
            FamilyName = info.Principal.GetFamilyName(),
            EntraIdSubjectId = info.Principal.GetEntraSubjectId(info.LoginProvider),
            OktaSubjectId = info.Principal.GetOktaSubjectId(info.LoginProvider),
            AccountCreatedAt = DateTimeOffset.Now,
            MostRecentLogin = DateTimeOffset.Now,
        };

        // Create the user in the backing store.
        var createUserResult = await userManager.CreateAsync(user);
        if (!createUserResult.Succeeded)
        {
            logger.LogWarning("Failed to create new user with subject ID {SubjectId}",
                info.Principal.GetSubjectId(info.LoginProvider) ?? "Unknown");
            return await FailedLoginAsync(createUserResult, user);
        }

        logger.LogInformation("Created new user with subject ID {SubjectId}",
            info.Principal.GetSubjectId(info.LoginProvider) ?? "Unknown");

        await SeedRolesAsync(user);

        // Add the external provider info to the user and sign in.
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            "Your account has successfully been created. Select “Edit Profile” to update your info.");
        return await AddLoginProviderAndSignInAsync(user, info, newUser: true);
    }

    private async Task SeedRolesAsync(ApplicationUser user)
    {
        // Add new user to application Roles if seeded in app settings.
        var settings = new List<SeedUserRoles>();
        configuration.GetSection(nameof(SeedUserRoles)).Bind(settings);
        var roles = settings.SingleOrDefault(userRoles =>
            userRoles.User.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase))?.Roles;
        if (roles is not null)
        {
            logger.LogInformation("Seeding roles for new user with object ID {ObjectId}", user.EntraIdSubjectId);
            foreach (var role in roles) await userManager.AddToRoleAsync(user, role);
        }
    }

    private sealed class SeedUserRoles
    {
        public string User { get; [UsedImplicitly] init; } = string.Empty;
        public List<string> Roles { get; [UsedImplicitly] init; } = null!;
    }

    // Update local store with from external provider. 
    private async Task<IActionResult> RefreshUserInfoAndSignInAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        logger.LogInformation("Existing user with object ID {ObjectId} logged in with {LoginProvider} provider",
            user.EntraIdSubjectId, info.LoginProvider);

        var previousValues = new ApplicationUser
        {
            UserName = user.UserName,
            Email = user.Email,
            GivenName = user.GivenName,
            FamilyName = user.FamilyName,
            OktaSubjectId = user.OktaSubjectId,
            EntraIdSubjectId = user.EntraIdSubjectId,
        };

        user.UserName = info.Principal.GetDisplayName();
        user.Email = info.Principal.GetEmail();
        user.GivenName = info.Principal.GetGivenName();
        user.FamilyName = info.Principal.GetFamilyName();
        user.OktaSubjectId = info.Principal.GetOktaSubjectId(info.LoginProvider) ?? previousValues.OktaSubjectId;
        user.EntraIdSubjectId = info.Principal.GetEntraSubjectId(info.LoginProvider) ?? previousValues.EntraIdSubjectId;
        user.MostRecentLogin = DateTimeOffset.Now;

        if (user.UserName != previousValues.UserName || user.Email != previousValues.Email ||
            user.GivenName != previousValues.GivenName || user.FamilyName != previousValues.FamilyName ||
            user.OktaSubjectId != previousValues.OktaSubjectId ||
            user.EntraIdSubjectId != previousValues.EntraIdSubjectId)
        {
            user.AccountUpdatedAt = DateTimeOffset.Now;
        }

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
            logger.LogWarning("Failed to add login provider {LoginProvider} for user with object ID {ObjectId}",
                info.LoginProvider, user.EntraIdSubjectId);
            return await FailedLoginAsync(addLoginResult, user);
        }

        user.OktaSubjectId ??= info.Principal.GetOktaSubjectId(info.LoginProvider);
        user.EntraIdSubjectId ??= info.Principal.GetEntraSubjectId(info.LoginProvider);
        user.MostRecentLogin = DateTimeOffset.Now;
        user.AccountUpdatedAt = DateTimeOffset.Now;
        await userManager.UpdateAsync(user);

        logger.LogInformation("Login provider {LoginProvider} added for user with object ID {ObjectId}",
            info.LoginProvider, user.EntraIdSubjectId);

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

    private IActionResult LocalRedirectOrHome() =>
        ReturnUrl is null ? RedirectToPage("/Staff/Index") : LocalRedirect(ReturnUrl);
}
