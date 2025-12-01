using Cts.AppServices.AuthenticationServices.Claims;
using Cts.Domain.Identity;
using GaEpd.AppLibrary.Domain.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace Cts.AppServices.AuthenticationServices;

public interface IAuthenticationManager
{
    public Task<IdentityResult> LogInUsingExternalProviderAsync();
    public Task<IdentityResult> LogInAsTestUserAsync(string[] testUserRoles);
}

public class AuthenticationManager(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    ILogger<AuthenticationManager> logger)
    : IAuthenticationManager
{
    public async Task<IdentityResult> LogInUsingExternalProviderAsync()
    {
        // Get information about the user from the external provider.
        var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync().ConfigureAwait(false);
        if (externalLoginInfo?.Principal is null)
            return MissingExternalLoginInfo();

        var loginProvider = externalLoginInfo.LoginProvider;
        var identityProviderId = externalLoginInfo.Principal.GetIdentityProviderId();
        var userEmail = externalLoginInfo.Principal.GetEmail();
        var providerKey = externalLoginInfo.ProviderKey;

        if (identityProviderId is null || userEmail is null)
            return MissingExternalLoginInfo();

        if (!configuration.ValidateLoginProviderId(loginProvider, identityProviderId))
            return InvalidLoginProvider(loginProvider, identityProviderId);

        logger.ZLogInformation($"User with ID {providerKey} in provider {loginProvider} successfully authenticated");

        // Find a user account using the external login provider.
        // If none, then find an account with the given username.
        var user = await userManager.FindByLoginAsync(loginProvider, providerKey).ConfigureAwait(false) ??
                   await userManager.FindByNameAsync(userEmail).ConfigureAwait(false);

        // If the user is not found, then create a new account.
        if (user is null)
            return await CreateUserAndSignInAsync(externalLoginInfo).ConfigureAwait(false);

        // If the user has been marked as inactive, don't sign in.
        if (!user.Active)
            return InactiveUser(providerKey);

        // Try to sign in the user locally with the external provider key.
        var signInResult = await signInManager
            .ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent: true)
            .ConfigureAwait(false);

        if (signInResult.IsLockedOut || signInResult.IsNotAllowed || signInResult.RequiresTwoFactor)
            return UserNotAllowed(providerKey);

        if (signInResult.Succeeded)
            return await RefreshUserInfoAndSignInAsync(user, externalLoginInfo).ConfigureAwait(false);

        // If the ExternalLoginInfo successfully returned from the external provider, and the user account already
        // exists, but ExternalLoginSignInAsync failed (`Succeeded == false`), then the user is likely using a new
        // external provider. Add the new provider info to the user account.
        return await AddLoginProviderAndSignInAsync(user, externalLoginInfo).ConfigureAwait(false);
    }

    public async Task<IdentityResult> LogInAsTestUserAsync(string[] testUserRoles)
    {
        const string userId = "00000000-0000-0000-0000-000000000001";
        var user = await userManager.FindByIdAsync(userId).ConfigureAwait(false);
        if (user is null) throw new EntityNotFoundException<ApplicationUser>(userId);
        logger.ZLogInformation($"Local user with ID {user.Id:@StaffId} signed in");

        foreach (var pair in AppRole.AllRoles)
            await userManager.RemoveFromRoleAsync(user, pair.Value.Name).ConfigureAwait(false);
        foreach (var role in testUserRoles)
            await userManager.AddToRoleAsync(user, role).ConfigureAwait(false);

        await signInManager.SignInWithClaimsAsync(user, isPersistent: false,
                additionalClaims: [new Claim(ClaimTypes.AuthenticationMethod, LoginProviders.TestUserScheme)])
            .ConfigureAwait(false);
        return IdentityResult.Success;
    }

    private async Task<IdentityResult> CreateUserAndSignInAsync(ExternalLoginInfo info)
    {
        var user = new ApplicationUser
        {
            UserName = info.Principal.GetDisplayName(),
            Email = info.Principal.GetEmail(),
            GivenName = info.Principal.GetGivenName(),
            FamilyName = info.Principal.GetFamilyName(),
            AccountCreatedAt = DateTimeOffset.Now,
            MostRecentLogin = DateTimeOffset.Now,
        };

        // Create the user in the backing store.
        var createUserResult = await userManager.CreateAsync(user).ConfigureAwait(false);
        if (!createUserResult.Succeeded)
            return UnableToCreateUser(info.ProviderKey);

        logger.ZLogInformation($"Created new user with ID {info.ProviderKey}");
        await SeedRolesAsync(user, info.LoginProvider).ConfigureAwait(false);

        return await AddLoginProviderAndSignInAsync(user, info).ConfigureAwait(false);
    }

    private async Task SeedRolesAsync(ApplicationUser user, string loginProvider)
    {
        if (loginProvider == LoginProviders.OktaScheme)
            await userManager.AddToRoleAsync(user, RoleName.Staff).ConfigureAwait(false);

        // Add the new user to application Roles if seeded in AppSettings.
        var settings = new List<SeedUserRoles>();
        configuration.GetSection(nameof(SeedUserRoles)).Bind(settings);
        var userRoles = settings.SingleOrDefault(s =>
            s.User.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase));

        if (userRoles is null) return;

        logger.ZLogInformation($"Seeding roles for new user with ID {user.Id}");
        foreach (var role in userRoles.Roles)
            await userManager.AddToRoleAsync(user, role).ConfigureAwait(false);
    }

    private sealed class SeedUserRoles
    {
        public string User { get; [UsedImplicitly] init; } = string.Empty;
        public List<string> Roles { get; [UsedImplicitly] init; } = null!;
    }

    private async Task<IdentityResult> AddLoginProviderAndSignInAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        // Add the external provider info to the user and sign in.
        var addLoginResult = await userManager.AddLoginAsync(user, info).ConfigureAwait(false);

        if (!addLoginResult.Succeeded)
            return UnableToAddLoginProvider(info.LoginProvider, info.ProviderKey);

        logger.ZLogInformation($"Login provider {info.LoginProvider} added for user with ID {info.ProviderKey}");

        // Update auditing info.
        user.MostRecentLogin = DateTimeOffset.Now;
        user.AccountUpdatedAt = DateTimeOffset.Now;
        await userManager.UpdateAsync(user).ConfigureAwait(false);

        return await FinalSignInAsync(user, info).ConfigureAwait(false);
    }

    private async Task<IdentityResult> FinalSignInAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        // Include the access token in the properties.
        var props = new AuthenticationProperties();
        if (info.AuthenticationTokens is not null) props.StoreTokens(info.AuthenticationTokens);
        props.IsPersistent = true;

        await signInManager.SignInAsync(user, props, info.LoginProvider).ConfigureAwait(false);
        return IdentityResult.Success;
    }

    private async Task<IdentityResult> RefreshUserInfoAndSignInAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        logger.ZLogInformation(
            $"Existing user with ID {info.ProviderKey} logged in with {info.LoginProvider} provider");

        var previousValues = new ApplicationUser
        {
            UserName = user.UserName,
            Email = user.Email,
            GivenName = user.GivenName,
            FamilyName = user.FamilyName,
        };

        user.UserName = info.Principal.GetDisplayName();
        user.Email = info.Principal.GetEmail();
        user.GivenName = info.Principal.GetGivenName();
        user.FamilyName = info.Principal.GetFamilyName();

        if (user.UserName != previousValues.UserName || user.Email != previousValues.Email ||
            user.GivenName != previousValues.GivenName || user.FamilyName != previousValues.FamilyName)
        {
            user.AccountUpdatedAt = DateTimeOffset.Now;
        }

        user.MostRecentLogin = DateTimeOffset.Now;
        await userManager.UpdateAsync(user).ConfigureAwait(false);

        return await FinalSignInAsync(user, info).ConfigureAwait(false);
    }

    // Identity Manager errors

    private IdentityResult MissingExternalLoginInfo()
    {
        const string description = "Error retrieving external account information";
        var error = new IdentityError
        {
            Code = nameof(MissingExternalLoginInfo),
            Description = $"{description}.",
        };
        logger.ZLogWarning($"{description}");
        return IdentityResult.Failed(error);
    }

    private IdentityResult InvalidLoginProvider(string loginProvider, string identityProviderId)
    {
        var error = new IdentityError
        {
            Code = nameof(InvalidLoginProvider),
            Description = $"Invalid login provider '{loginProvider}' with ID '{identityProviderId}'.",
        };
        logger.ZLogWarning($"Invalid login provider '{loginProvider}' with ID '{identityProviderId}'");
        return IdentityResult.Failed(error);
    }

    private IdentityResult UnableToCreateUser(string subjectId)
    {
        var error = new IdentityError
        {
            Code = nameof(UnableToCreateUser),
            Description = $"Failed to create new user with subject ID {subjectId}.",
        };
        logger.ZLogWarning($"Failed to create new user with subject ID {subjectId}");
        return IdentityResult.Failed(error);
    }

    private IdentityResult UnableToAddLoginProvider(string loginProvider, string providerKey)
    {
        var error = new IdentityError
        {
            Code = nameof(UnableToAddLoginProvider),
            Description = $"Failed to add login provider {loginProvider} for user with ID {providerKey}.",
        };
        logger.ZLogWarning($"Failed to add login provider {loginProvider} for user with ID {providerKey}");
        return IdentityResult.Failed(error);
    }

    private IdentityResult InactiveUser(string subjectId)
    {
        var error = new IdentityError
        {
            Code = nameof(InactiveUser),
            Description = $"Inactive user with subject ID {subjectId}.",
        };
        logger.ZLogWarning($"Inactive user with subject ID {subjectId}");
        return IdentityResult.Failed(error);
    }

    private IdentityResult UserNotAllowed(string subjectId)
    {
        var error = new IdentityError
        {
            Code = nameof(UserNotAllowed),
            Description = $"User with subject ID {subjectId} is not allowed.",
        };
        logger.ZLogWarning($"User with subject ID {subjectId} is not allowed");
        return IdentityResult.Failed(error);
    }
}
