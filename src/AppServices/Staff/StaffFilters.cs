using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using GaEpd.AppLibrary.Enums;
using GaEpd.AppLibrary.Pagination;

namespace Cts.AppServices.Staff;

public static class StaffFilters
{
    public static IQueryable<ApplicationUser> ApplyFilter(
        this IQueryable<ApplicationUser> userQuery, StaffSearchDto spec) =>
        userQuery.FilterByName(spec.Name)
            .FilterByEmail(spec.Email)
            .FilterByOffice(spec.Office)
            .FilterByActiveStatus(spec.Status)
            .OrderByIf(spec.Sort.GetDescription());

    private static IQueryable<ApplicationUser> FilterByName(
        this IQueryable<ApplicationUser> query, string? name) =>
        string.IsNullOrWhiteSpace(name)
            ? query
            : query.Where(m => m.GivenName.ToLower().Contains(name.ToLower())
                || m.FamilyName.ToLower().Contains(name.ToLower()));

    private static IQueryable<ApplicationUser> FilterByEmail(
        this IQueryable<ApplicationUser> query, string? email) =>
        string.IsNullOrWhiteSpace(email) ? query : query.Where(m => m.Email == email);

    private static IQueryable<ApplicationUser> FilterByOffice(
        this IQueryable<ApplicationUser> query, Guid? officeId) =>
        officeId is null ? query : query.Where(m => m.Office != null && m.Office.Id == officeId);

    public static IQueryable<ApplicationUser> FilterByActiveStatus(
        this IQueryable<ApplicationUser> query, SearchStaffStatus? status) =>
        status == SearchStaffStatus.All
            ? query
            : query.Where(m => m.Active == (status == null || status == SearchStaffStatus.Active));
}
