using Cts.Domain.Identity;
using System.Security.Claims;
using System.Security.Principal;

namespace Cts.AppServices.Permissions.Helpers;

public static class PrincipalExtensions
{
    public static string? GetUserIdValue(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.NameIdentifier);

    internal static bool IsActive(this ClaimsPrincipal principal) =>
        principal.HasClaim(claim => claim.Type == nameof(Policies.ActiveUser) && claim.Value == true.ToString());

    private static bool IsInRoles(this IPrincipal principal, IEnumerable<string> roles) =>
        roles.All(principal.IsInRole);

    internal static bool IsAttachmentsEditor(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.AttachmentsEditor, RoleName.DivisionManager]);

    internal static bool IsDataExporter(this IPrincipal principal) =>
        principal.IsInRole(RoleName.DataExport);

    internal static bool IsDivisionManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.DivisionManager);

    internal static bool IsManager(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.Manager, RoleName.DivisionManager]);

    internal static bool IsSiteMaintainer(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.SiteMaintenance, RoleName.DivisionManager]);

    internal static bool IsStaff(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.Staff, RoleName.Manager, RoleName.DivisionManager]);

    internal static bool IsUserAdmin(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.UserAdmin, RoleName.DivisionManager]);
}
