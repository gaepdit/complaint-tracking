using Cts.AppServices.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace AppServicesTests.Permissions.PolicyTests;

public class ActiveUserPolicy
{
    private IAuthorizationService _authorizationService = null!;

    [SetUp]
    public void SetUp() => _authorizationService = AuthorizationServiceBuilder.BuildAuthorizationService(collection =>
        collection.AddAuthorization(options =>
            options.AddPolicy(nameof(Policies.ActiveUser), Policies.ActiveUser)));

    [Test]
    public async Task WhenActiveAndAuthenticated_Succeeds()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(nameof(Policies.ActiveUser), true.ToString()), },
            "Basic"));
        var result = (await _authorizationService.AuthorizeAsync(user, Policies.ActiveUser)).Succeeded;
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotActive_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var result = (await _authorizationService.AuthorizeAsync(user, Policies.ActiveUser)).Succeeded;
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNotAuthenticated_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(nameof(Policies.ActiveUser), true.ToString()), }));
        var result = (await _authorizationService.AuthorizeAsync(user, Policies.ActiveUser)).Succeeded;
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNeitherActiveNorAuthenticated_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var result = (await _authorizationService.AuthorizeAsync(user, Policies.ActiveUser)).Succeeded;
        result.Should().BeFalse();
    }
}
