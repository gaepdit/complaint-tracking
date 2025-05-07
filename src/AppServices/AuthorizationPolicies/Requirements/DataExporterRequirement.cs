using Cts.AppServices.IdentityServices.Roles;
using Microsoft.AspNetCore.Authorization;

namespace Cts.AppServices.AuthorizationPolicies.Requirements;

internal class DataExporterRequirement :
    AuthorizationHandler<DataExporterRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DataExporterRequirement requirement)
    {
        if (context.User.IsDataExporter())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
