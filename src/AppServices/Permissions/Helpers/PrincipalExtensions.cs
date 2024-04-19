﻿using Cts.AppServices.Permissions.AppClaims;
using Cts.Domain.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;

namespace Cts.AppServices.Permissions.Helpers;

public static class PrincipalExtensions
{
    public static string? GetUserIdValue(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.NameIdentifier);

    public static bool HasRealClaim(this ClaimsPrincipal principal, string type, [NotNullWhen(true)] string? value) =>
        value is not null && principal.HasClaim(type, value);

    internal static bool IsActive(this ClaimsPrincipal principal) =>
        principal.HasClaim(AppClaimTypes.ActiveUser, true.ToString());

    private static bool IsInRoles(this IPrincipal principal, IEnumerable<string> roles) =>
        roles.Any(principal.IsInRole);

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
