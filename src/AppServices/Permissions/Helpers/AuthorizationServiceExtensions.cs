using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Principal;

namespace Cts.AppServices.Permissions.Helpers;

public static class AuthorizationServiceExtensions
{
    public static async Task<bool> Succeeded(this IAuthorizationService service, ClaimsPrincipal user, object? resource,
        IAuthorizationRequirement requirement) =>
        (await service.AuthorizeAsync(user, resource, requirement).ConfigureAwait(false)).Succeeded;

    public static async Task<bool> Succeeded(this IAuthorizationService service, ClaimsPrincipal user,
        AuthorizationPolicy policy) =>
        (await service.AuthorizeAsync(user, policy).ConfigureAwait(false)).Succeeded;

    public static async Task<bool> Succeeded(this IAuthorizationService service, IPrincipal user,
        AuthorizationPolicy policy) =>
        (await service.AuthorizeAsync((ClaimsPrincipal)user, policy).ConfigureAwait(false)).Succeeded;
}
