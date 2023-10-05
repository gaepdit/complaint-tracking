using Microsoft.AspNetCore.Authorization;
using Cts.AppServices.Permissions.Helpers;

namespace Cts.AppServices.Permissions.Requirements;

internal class AttachmentsEditorRequirement :
    AuthorizationHandler<AttachmentsEditorRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AttachmentsEditorRequirement requirement)
    {
        if (context.User.IsAttachmentsEditor())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
