using Cts.Domain.Entities;
using Cts.Domain.Offices;
using Cts.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cts.Infrastructure.Repositories;

public sealed class OfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public OfficeRepository(CtsDbContext dbContext) : base(dbContext) { }

    public Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        Context.Offices.AsNoTracking().SingleOrDefaultAsync(e => e.Name == name, token);

    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token)).StaffMembers.Where(e => e.Active).ToList();
}
