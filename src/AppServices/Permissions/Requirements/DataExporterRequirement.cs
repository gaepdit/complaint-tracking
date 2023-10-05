using Microsoft.AspNetCore.Authorization;
using Cts.AppServices.Permissions.Helpers;

namespace Cts.AppServices.Permissions.Requirements;

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
