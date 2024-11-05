using Cts.AppServices.Permissions.AppClaims;
using Cts.Domain.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;

namespace Cts.AppServices.Permissions.Helpers;

public static class PrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Email);

    public static string GetGivenName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;

    public static string GetFamilyName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;

    public static bool HasRealClaim(this ClaimsPrincipal principal, string type, [NotNullWhen(true)] string? value) =>
        value is not null && principal.HasClaim(type, value);

    internal static bool IsActive(this ClaimsPrincipal principal) =>
        principal.HasClaim(AppClaimTypes.ActiveUser, true.ToString());

    private static bool IsInOneOfRoles(this IPrincipal principal, IEnumerable<string> roles) =>
        roles.Any(principal.IsInRole);

    internal static bool IsAttachmentsEditor(this IPrincipal principal) =>
        principal.IsInOneOfRoles([RoleName.AttachmentsEditor, RoleName.DivisionManager]);

    internal static bool IsDataExporter(this IPrincipal principal) =>
        principal.IsInRole(RoleName.DataExport);

    internal static bool IsDivisionManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.DivisionManager);

    internal static bool IsManager(this IPrincipal principal) =>
        principal.IsInOneOfRoles([RoleName.Manager, RoleName.DivisionManager]);

    internal static bool IsSiteMaintainer(this IPrincipal principal) =>
        principal.IsInOneOfRoles([RoleName.SiteMaintenance, RoleName.DivisionManager]);

    internal static bool IsStaff(this IPrincipal principal) =>
        principal.IsInOneOfRoles([RoleName.Staff, RoleName.Manager, RoleName.DivisionManager]);

    internal static bool IsUserAdmin(this IPrincipal principal) =>
        principal.IsInOneOfRoles([RoleName.UserAdmin, RoleName.SuperUserAdmin, RoleName.DivisionManager]);

    internal static bool IsSuperUserAdmin(this IPrincipal principal) =>
        principal.IsInOneOfRoles([RoleName.SuperUserAdmin, RoleName.DivisionManager]);
}
