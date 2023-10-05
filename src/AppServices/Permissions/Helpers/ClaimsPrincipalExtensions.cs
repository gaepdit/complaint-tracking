using System.Security.Claims;

namespace Cts.AppServices.Permissions.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserIdValue(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.NameIdentifier);
}
