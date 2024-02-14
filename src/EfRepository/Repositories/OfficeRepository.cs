using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;

namespace Cts.EfRepository.Repositories;

public sealed class OfficeRepository(AppDbContext context) :
    NamedEntityRepository<Office, AppDbContext>(context), IOfficeRepository
{
    public Task<List<ApplicationUser>> GetStaffMembersListAsync(Guid id, bool includeInactive,
        CancellationToken token = default) =>
        Context.Set<ApplicationUser>().AsNoTracking()
            .Where(user => user.Office != null && user.Office.Id == id)
            .Where(user => includeInactive || user.Active)
            .OrderBy(user => user.FamilyName).ThenBy(user => user.GivenName).ThenBy(user => user.Id)
            .ToListAsync(token);

    public Task<Office?> FindIncludeAssignorAsync(Guid id, CancellationToken token = default) =>
        Context.Set<Office>().AsNoTracking()
            .Include(office => office.Assignor)
            .SingleOrDefaultAsync(office => office.Id.Equals(id), token);

    public async Task<IReadOnlyCollection<Office>> GetListIncludeAssignorAsync(CancellationToken token = default) =>
        await Context.Set<Office>().AsNoTracking()
            .Include(office => office.Assignor)
            .OrderBy(office => office.Name).ThenBy(office => office.Id)
            .ToListAsync(token).ConfigureAwait(false);
}
