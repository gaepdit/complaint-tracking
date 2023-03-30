﻿using Cts.AppServices.Permissions.Requirements;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.Permissions;

public class SiteMaintainerPermissions
{
    [Test]
    public async Task BySiteMaintainer_Succeeds()
    {
        var requirements = new[] { new SiteMaintainerRequirement() };
        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.SiteMaintenance) },
            "Basic"));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new SiteMaintainerRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task ByDivisionManager_Succeeds()
    {
        var requirements = new[] { new SiteMaintainerRequirement() };
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.DivisionManager) },
            "Basic"));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new SiteMaintainerRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotAuthenticated_DoesNotSucceed()
    {
        var requirements = new[] { new SiteMaintainerRequirement() };
        // This `ClaimsPrincipal` is not authenticated.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.SiteMaintenance) }));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new SiteMaintainerRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task WhenNotAllowed_DoesNotSucceed()
    {
        var requirements = new[] { new SiteMaintainerRequirement() };
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var context = new AuthorizationHandlerContext(requirements, user, null);
        var handler = new SiteMaintainerRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }
}
