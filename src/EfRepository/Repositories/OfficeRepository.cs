using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.EfRepository.Contexts;
using GaEpd.AppLibrary.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cts.EfRepository.Repositories;

public sealed class OfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public OfficeRepository(AppDbContext context) : base(context) { }

    public async Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        await Context.Offices.AsNoTracking()
            .SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);

    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(
        Guid id, CancellationToken token = default)
    {
        var item = await Context.Offices.AsTracking()
            .Include(e => e.StaffMembers)
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);
        return item is null
            ? throw new EntityNotFoundException(typeof(Office), id)
            : item.StaffMembers.Where(e => e.Active)
                .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName).ToList();
    }

    // Hide some base repository methods in order to include additional data.

    public new async Task<Office?> FindAsync(Guid id, CancellationToken token = default) =>
        await Context.Set<Office>().AsNoTracking()
            .Include(e => e.Assignor)
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);

    public new async Task<IReadOnlyCollection<Office>> GetListAsync(CancellationToken token = default) =>
        await Context.Set<Office>().AsNoTracking()
            .Include(e => e.Assignor).ToListAsync(token);
}
