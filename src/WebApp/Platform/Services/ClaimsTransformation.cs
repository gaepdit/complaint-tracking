using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Cts.AppServices.Permissions;
using Cts.Domain.Identity;
using System.Security.Claims;

namespace Cts.WebApp.Platform.Services;

public class ClaimsTransformation : IClaimsTransformation
{
    private readonly UserManager<ApplicationUser> _userManager;
    public ClaimsTransformation(UserManager<ApplicationUser> userManager) => _userManager = userManager;

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var userIsActive = (await _userManager.GetUserAsync(principal))?.Active ?? false;

        if (principal.HasClaim(nameof(Policies.ActiveUser), userIsActive.ToString()))
            return principal;

        foreach (var identity in principal.Identities)
            identity.TryRemoveClaim(identity.FindFirst(nameof(Policies.ActiveUser)));

        (principal.Identity as ClaimsIdentity)!.AddClaim(new Claim(nameof(Policies.ActiveUser), userIsActive.ToString()));

        return principal;
    }
}
