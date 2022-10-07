using MyAppRoot.Domain.Identity;

namespace MyAppRoot.AppServices.Staff;

public static class StaffFilters
{
    public static List<ApplicationUser> ApplyFilter(
        this IQueryable<ApplicationUser> userQuery, StaffSearchDto filter) =>
        userQuery.FilterByName(filter.Name)
            .FilterByEmail(filter.Email)
            .FilterByOffice(filter.Office)
            .FilterByActiveStatus(filter.Status)
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .ToList();

    private static IQueryable<ApplicationUser> FilterByName(
        this IQueryable<ApplicationUser> query, string? name) =>
        string.IsNullOrWhiteSpace(name)
            ? query
            : query.Where(m => m.FirstName.ToLower().Contains(name.ToLower())
                || m.LastName.ToLower().Contains(name.ToLower()));

    private static IQueryable<ApplicationUser> FilterByEmail(
        this IQueryable<ApplicationUser> query, string? email) =>
        string.IsNullOrWhiteSpace(email) ? query : query.Where(m => m.Email == email);

    private static IQueryable<ApplicationUser> FilterByOffice(
        this IQueryable<ApplicationUser> query, Guid? officeId) =>
        officeId is null ? query : query.Where(m => m.Office != null && m.Office.Id == officeId);

    private static IQueryable<ApplicationUser> FilterByActiveStatus(
        this IQueryable<ApplicationUser> query, StaffSearchDto.ActiveStatus status) =>
        status == StaffSearchDto.ActiveStatus.All
            ? query
            : query.Where(m => m.Active == (status == StaffSearchDto.ActiveStatus.Active));
}
