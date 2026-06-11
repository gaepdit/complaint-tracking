using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Cts.AppServices.AuthenticationServices;

public static class AppClaimTypes
{
    // App claim types
    public const string ActiveUser = nameof(ActiveUser);
    public const string OfficeId = nameof(OfficeId);
}

public class AppClaimsTransformation(UserManager<ApplicationUser> userManager) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var applicationUser = await userManager.GetUserAsync(principal).ConfigureAwait(false);
        if (applicationUser is null) return principal;

        var claimsIdentity = new ClaimsIdentity();
        AddNewClaim(claimsIdentity, principal, AppClaimTypes.ActiveUser, applicationUser.Active.ToString());
        AddNewClaim(claimsIdentity, principal, AppClaimTypes.OfficeId, applicationUser.Office?.Id.ToString());

        principal.AddIdentity(claimsIdentity);
        return principal;
    }

    private static void AddNewClaim(ClaimsIdentity claimsIdentity, ClaimsPrincipal principal,
        string type, string? value)
    {
        if (value != null && !principal.HasClaim(type, value)) claimsIdentity.AddClaim(new Claim(type, value));
    }
}
