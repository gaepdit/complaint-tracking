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
        var requirements = new[] { new SiteMaintainerRequirement() };
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.SiteMaintenance) }));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new SiteMaintainerRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task WhenOnlyUserAdmin_DoesNotSucceed()
    {
        var requirements = new[] { new SiteMaintainerRequirement() };
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.UserAdmin) }));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new SiteMaintainerRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task WhenNoRoles_DoesNotSucceed()
    {
        var requirements = new[] { new SiteMaintainerRequirement() };
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new SiteMaintainerRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }
}
