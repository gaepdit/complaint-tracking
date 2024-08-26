using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.Complaints.Permissions;

public class ComplaintViewPermissions
{
    [Test]
    public async Task ManageDeletions_WhenAllowed_Succeeds()
    {
        var requirements = new[] { ComplaintOperation.ManageDeletions };
        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.DivisionManager)],
            "Basic"));
        var resource = new ComplaintViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new ComplaintViewRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAuthenticated_DoesNotSucceed()
    {
        var requirements = new[] { ComplaintOperation.ManageDeletions };
        // This `ClaimsPrincipal` is not authenticated.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.DivisionManager)]));
        var resource = new ComplaintViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new ComplaintViewRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAllowed_DoesNotSucceed()
    {
        var requirements = new[] { ComplaintOperation.ManageDeletions };
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var resource = new ComplaintViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new ComplaintViewRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    // Original reporter can edit for 1 hour.
    [Test]
    public async Task RecentReporter_IfEnteredWithinPastHour_Succeeds()
    {
        var requirements = new[] { ComplaintOperation.EditDetails };
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString()),
                new Claim(ClaimTypes.Role, RoleName.Staff),
            ],
            "Basic"));
        var resource = new ComplaintViewDto
        {
            EnteredDate = DateTimeOffset.Now.AddMinutes(-30),
            EnteredBy = new StaffViewDto { Id = Guid.Empty.ToString() },
        };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new ComplaintViewRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task RecentReporter_IfEnteredBeforePastHour_DoesNotSucceed()
    {
        var requirements = new[] { ComplaintOperation.EditDetails };
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString()),
                new Claim(ClaimTypes.Role, RoleName.Staff),
            ],
            "Basic"));
        var resource = new ComplaintViewDto
        {
            EnteredDate = DateTimeOffset.Now.AddMinutes(-90),
            EnteredBy = new StaffViewDto { Id = Guid.Empty.ToString() },
        };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new ComplaintViewRequirement();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }
}
