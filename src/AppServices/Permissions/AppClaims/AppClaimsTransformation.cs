using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Cts.AppServices.Permissions.AppClaims;

public class AppClaimsTransformation(UserManager<ApplicationUser> userManager) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var applicationUser = await userManager.GetUserAsync(principal).ConfigureAwait(false);

        var claimsIdentity = new ClaimsIdentity();
        AddNewClaim(claimsIdentity, principal, AppClaimTypes.ActiveUser, applicationUser?.Active.ToString());
        AddNewClaim(claimsIdentity, principal, AppClaimTypes.OfficeId, applicationUser?.Office?.Id.ToString());

        principal.AddIdentity(claimsIdentity);
        return principal;
    }

    private static void AddNewClaim(ClaimsIdentity claimsIdentity, ClaimsPrincipal principal,
        string type, string? value)
    {
        if (value != null && !principal.HasClaim(type, value)) claimsIdentity.AddClaim(new Claim(type, value));
    }
}
