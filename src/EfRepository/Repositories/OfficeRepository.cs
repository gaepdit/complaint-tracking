using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.EfRepository.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cts.EfRepository.Repositories;

public sealed class OfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public OfficeRepository(AppDbContext context) : base(context) { }

    public async Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        await Context.Offices.AsNoTracking()
            .SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);

    public async Task<List<ApplicationUser>> GetStaffMembersListAsync(
        Guid id, bool activeOnly, CancellationToken token = default) =>
        await Context.Users.AsNoTracking()
            .Where(e => e.Office != null && e.Office.Id == id)
            .Where(e => !activeOnly || e.Active)
            .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName)
            .ToListAsync(token);

    // Hide some base repository methods in order to include additional data.

    public new async Task<Office?> FindAsync(Guid id, CancellationToken token = default) =>
        await Context.Set<Office>().AsNoTracking()
            .Include(e => e.Assignor)
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);

    public new async Task<IReadOnlyCollection<Office>> GetListAsync(CancellationToken token = default) =>
        await Context.Set<Office>().AsNoTracking()
            .Include(e => e.Assignor).ToListAsync(token);
}
