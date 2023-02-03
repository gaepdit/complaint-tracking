using Cts.Domain.Identity;

namespace Cts.AppServices.Staff;

public static class StaffFilters
{
    public static IQueryable<ApplicationUser> ApplyFilter(
        this IQueryable<ApplicationUser> userQuery, StaffSearchDto filter) =>
        userQuery.FilterByName(filter.Name)
            .FilterByEmail(filter.Email)
            .FilterByOffice(filter.Office)
            .FilterByActiveStatus(filter.Status)
            .OrderBy(m => m.FamilyName)
            .ThenBy(m => m.GivenName);

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
        this IQueryable<ApplicationUser> query, StaffSearchDto.ActiveStatus status) =>
        status == StaffSearchDto.ActiveStatus.All
            ? query
            : query.Where(m => m.Active == (status == StaffSearchDto.ActiveStatus.Active));
}
