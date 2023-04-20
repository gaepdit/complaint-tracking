using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.EfRepository.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cts.EfRepository.Repositories;

/// <inheritdoc cref="IOfficeRepository"/>
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

    public async Task<Office?> FindIncludeAssignorAsync(Guid id, CancellationToken token = default) =>
        await Context.Set<Office>().AsNoTracking()
            .Include(e => e.Assignor)
            .SingleOrDefaultAsync(e => e.Id.Equals(id), token);

    public async Task<IReadOnlyCollection<Office>> GetListIncludeAssignorAsync(CancellationToken token = default) =>
        await Context.Set<Office>().AsNoTracking()
            .Include(e => e.Assignor).ToListAsync(token);
}
