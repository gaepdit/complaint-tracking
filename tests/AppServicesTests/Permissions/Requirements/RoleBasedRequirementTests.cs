using Cts.AppServices.Permissions.Requirements;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.Permissions.Requirements;

public class RoleBasedRequirementTests
{
    [Test]
    public async Task WhenDivisionManager_Succeeds()
    {
        var handler = new SiteMaintainerRequirement();
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.SiteMaintenance) }));
        var context = new AuthorizationHandlerContext([handler], user, null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task WhenOnlyUserAdmin_DoesNotSucceed()
    {
        var handler = new SiteMaintainerRequirement();
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.UserAdmin) }));
        var context = new AuthorizationHandlerContext([handler], user, null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task WhenNoRoles_DoesNotSucceed()
    {
        var handler = new SiteMaintainerRequirement();
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var context = new AuthorizationHandlerContext([handler], user, null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }
}
