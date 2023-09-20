using Cts.AppServices.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace AppServicesTests.Permissions.PolicyTests;

public class LoggedInUserPolicy
{
    private IAuthorizationService _authorizationService = null!;

    [SetUp]
    public void SetUp() => _authorizationService = AuthorizationServiceBuilder.BuildAuthorizationService(collection =>
        collection.AddAuthorization(options =>
            options.AddPolicy(nameof(Policies.LoggedInUser), Policies.LoggedInUser)));

    [Test]
    public async Task WhenAuthenticated_Succeeds()
    {
        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var result = (await _authorizationService.AuthorizeAsync(user, Policies.LoggedInUser)).Succeeded;
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotAuthenticated_DoesNotSucceed()
    {
        // This `ClaimsPrincipal` is not authenticated.
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var result = (await _authorizationService.AuthorizeAsync(user, Policies.LoggedInUser)).Succeeded;
        result.Should().BeFalse();
    }
}
