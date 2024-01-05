using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;

namespace Cts.EfRepository.Repositories;

public sealed class OfficeRepository(AppDbContext context) :
    NamedEntityRepository<Office, AppDbContext>(context), IOfficeRepository
{
    public Task<List<ApplicationUser>> GetStaffMembersListAsync(Guid id, bool includeInactive,
        CancellationToken token = default) =>
        Context.Set<ApplicationUser>().AsNoTracking()
            .Where(e => e.Office != null && e.Office.Id == id)
            .Where(e => includeInactive || e.Active)
            .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName)
            .ToListAsync(token);

    public Task<Office?> FindIncludeAssignorAsync(Guid id, CancellationToken token = default) =>
        Context.Set<Office>().AsNoTracking()
            .Include(e => e.Assignor)
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);

    public async Task<IReadOnlyCollection<Office>> GetListIncludeAssignorAsync(CancellationToken token = default) =>
        await Context.Set<Office>().AsNoTracking()
            .Include(e => e.Assignor)
            .ToListAsync(token);
}
