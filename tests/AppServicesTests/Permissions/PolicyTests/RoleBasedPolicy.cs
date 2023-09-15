using Cts.AppServices.Permissions;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace AppServicesTests.Permissions.PolicyTests;

public class RoleBasedPolicy
{
    private IAuthorizationService _authorizationService = null!;

    [SetUp]
    public void SetUp() => _authorizationService = AuthorizationServiceBuilder.BuildAuthorizationService(collection =>
        collection.AddAuthorization(options =>
            options.AddPolicy(nameof(Policies.SiteMaintainer), Policies.SiteMaintainer)));

    [Test]
    public async Task WhenAuthenticatedAndActiveAndDivisionManager_Succeeds()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[]
            {
                new(nameof(Policies.ActiveUser), true.ToString()),
                new(ClaimTypes.Role, RoleName.SiteMaintenance),
            }, "Basic"));
        var result = (await _authorizationService.AuthorizeAsync(user, Policies.SiteMaintainer)).Succeeded;
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotActive_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[]
            {
                new(ClaimTypes.Role, RoleName.SiteMaintenance),
            }, "Basic"));
        var result = (await _authorizationService.AuthorizeAsync(user, Policies.SiteMaintainer)).Succeeded;
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNotDivisionManager_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[]
            {
                new(nameof(Policies.ActiveUser), true.ToString()),
            }, "Basic"));
        var result = (await _authorizationService.AuthorizeAsync(user, Policies.SiteMaintainer)).Succeeded;
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNotAuthenticated_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[]
            {
                new(nameof(Policies.ActiveUser), true.ToString()),
                new(ClaimTypes.Role, RoleName.SiteMaintenance),
            }));
        var result = (await _authorizationService.AuthorizeAsync(user, Policies.SiteMaintainer)).Succeeded;
        result.Should().BeFalse();
    }
}
