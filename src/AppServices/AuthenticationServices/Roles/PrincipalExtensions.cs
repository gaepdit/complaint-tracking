using Cts.Domain.Identity;
using System.Security.Principal;

namespace Cts.AppServices.AuthenticationServices.Roles;

public static class PrincipalExtensions
{
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
