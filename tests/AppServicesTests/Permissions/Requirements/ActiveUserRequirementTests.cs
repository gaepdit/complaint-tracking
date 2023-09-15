using Cts.AppServices.Permissions.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.Permissions.Requirements;

public class ActiveUserRequirementTests
{
    [Test]
    public async Task WhenActive_Succeeds()
    {
        var requirements = new[] { new ActiveUserRequirement() };
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(nameof(Cts.AppServices.Permissions.Policies.ActiveUser), true.ToString()) }));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new ActiveUserRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotActive_DoesNotSucceed()
    {
        var requirements = new[] { new ActiveUserRequirement() };
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new ActiveUserRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }
}
