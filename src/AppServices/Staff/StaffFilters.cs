using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using GaEpd.AppLibrary.Extensions;
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
            .OrderByIf(spec.Sort.GetDescription())
            .ThenBy(user => user.Id);

#pragma warning disable CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparisons
    // The 'StringComparison' method overload is incompatible with Entity Framework.
    private static IQueryable<ApplicationUser> FilterByName(
        this IQueryable<ApplicationUser> query, string? name) =>
        string.IsNullOrWhiteSpace(name)
            ? query
            : query.Where(user => user.GivenName.ToLower().Contains(name.ToLower())
                || user.FamilyName.ToLower().Contains(name.ToLower()));
#pragma warning restore CA1862

    private static IQueryable<ApplicationUser> FilterByEmail(
        this IQueryable<ApplicationUser> query, string? email) =>
        string.IsNullOrWhiteSpace(email) ? query : query.Where(user => user.Email == email);

    private static IQueryable<ApplicationUser> FilterByOffice(
        this IQueryable<ApplicationUser> query, Guid? officeId) =>
        officeId is null ? query : query.Where(user => user.Office != null && user.Office.Id == officeId);

    private static IQueryable<ApplicationUser> FilterByActiveStatus(
        this IQueryable<ApplicationUser> query, SearchStaffStatus? status) =>
        status == SearchStaffStatus.All
            ? query
            : query.Where(user => user.Active == (status == null || status == SearchStaffStatus.Active));
}
