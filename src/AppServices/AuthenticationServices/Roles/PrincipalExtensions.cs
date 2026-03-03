using Cts.Domain.Identity;
using System.Security.Principal;

namespace Cts.AppServices.AuthenticationServices.Roles;

public static class PrincipalExtensions
{
    extension(IPrincipal principal)
    {
        private bool IsInOneOfRoles(IEnumerable<string> roles) => roles.Any(principal.IsInRole);

        internal bool IsAttachmentsEditor() =>
            principal.IsInOneOfRoles([RoleName.AttachmentsEditor, RoleName.DivisionManager]);

        internal bool IsDataExporter() => principal.IsInRole(RoleName.DataExport);

        internal bool IsDivisionManager() => principal.IsInRole(RoleName.DivisionManager);

        internal bool IsManager() => principal.IsInOneOfRoles([RoleName.Manager, RoleName.DivisionManager]);

        internal bool IsSiteMaintainer() =>
            principal.IsInOneOfRoles([RoleName.SiteMaintenance, RoleName.DivisionManager]);

        internal bool IsStaff() =>
            principal.IsInOneOfRoles([RoleName.Staff, RoleName.Manager, RoleName.DivisionManager]);

        internal bool IsUserAdmin() =>
            principal.IsInOneOfRoles([RoleName.UserAdmin, RoleName.SuperUserAdmin, RoleName.DivisionManager]);

        internal bool IsSuperUserAdmin() =>
            principal.IsInOneOfRoles([RoleName.SuperUserAdmin, RoleName.DivisionManager]);
    }
}
