using Cts.Domain.Identity;
using System.Security.Claims;
using System.Security.Principal;

namespace Cts.AppServices.Permissions.Helpers;

public static class UserRequirements
{
    internal static bool IsActive(this ClaimsPrincipal user) =>
        user.HasClaim(claim => claim.Type == nameof(Policies.ActiveUser) && claim.Value == true.ToString());

    internal static bool IsAttachmentsEditor(this IPrincipal user) =>
        user.IsInRole(RoleName.AttachmentsEditor) || user.IsDivisionManager();

    internal static bool IsDataExporter(this IPrincipal user) => user.IsInRole(RoleName.DataExport);

    internal static bool IsDivisionManager(this IPrincipal user) => user.IsInRole(RoleName.DivisionManager);

    internal static bool IsManager(this IPrincipal user) => user.IsInRole(RoleName.Manager) || user.IsDivisionManager();

    internal static bool IsSiteMaintainer(this IPrincipal user) =>
        user.IsInRole(RoleName.SiteMaintenance) || user.IsDivisionManager();

    internal static bool IsStaff(this IPrincipal user) => user.IsInRole(RoleName.Staff) || user.IsManager();

    internal static bool IsUserAdmin(this IPrincipal user) =>
        user.IsInRole(RoleName.UserAdmin) || user.IsDivisionManager();
}
