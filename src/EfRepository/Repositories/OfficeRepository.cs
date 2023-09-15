using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.EfRepository.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cts.EfRepository.Repositories;

public sealed class OfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public OfficeRepository(AppDbContext context) : base(context) { }

    public Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        Context.Offices.AsNoTracking()
            .SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);

    public Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(Guid id, CancellationToken token = default) =>
        Context.Users.AsNoTracking()
            .Where(e => e.Office != null && e.Office.Id == id)
            .Where(e => e.Active)
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
