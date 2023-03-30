using Cts.AppServices.Permissions.Requirements;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.Permissions;

public class DivisionManagerPermissions
{
    [Test]
    public async Task ByDivisionManager_Succeeds()
    {
        var requirements = new[] { new DivisionManagerRequirement() };
        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.DivisionManager) },
            "Basic"));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new DivisionManagerRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task ByUserAdmin_DoesNotSucceed()
    {
        var requirements = new[] { new DivisionManagerRequirement() };
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.UserAdmin) },
            "Basic"));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new DivisionManagerRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task WhenNotAuthenticated_DoesNotSucceed()
    {
        var requirements = new[] { new DivisionManagerRequirement() };
        // This `ClaimsPrincipal` is not authenticated.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.DivisionManager) }));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new DivisionManagerRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task WhenNotAllowed_DoesNotSucceed()
    {
        var requirements = new[] { new DivisionManagerRequirement() };
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new DivisionManagerRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }
}
