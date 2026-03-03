using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Principal;

namespace Cts.AppServices.AuthorizationPolicies;

public static class AuthorizationServiceExtensions
{
    extension(IAuthorizationService service)
    {
        public async Task<bool> Succeeded(ClaimsPrincipal user, object? resource,
            IAuthorizationRequirement requirement) =>
            (await service.AuthorizeAsync(user, resource, requirement).ConfigureAwait(false)).Succeeded;

        public async Task<bool> Succeeded(ClaimsPrincipal user, AuthorizationPolicy policy) =>
            (await service.AuthorizeAsync(user, policy).ConfigureAwait(false)).Succeeded;

        public async Task<bool> Succeeded(IPrincipal user, AuthorizationPolicy policy) =>
            (await service.AuthorizeAsync((ClaimsPrincipal)user, policy).ConfigureAwait(false)).Succeeded;
    }
}
