using Cts.Domain.Entities.Concerns;
using Cts.EfRepository.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cts.EfRepository.Repositories;

public sealed class ConcernRepository : BaseRepository<Concern, Guid>, IConcernRepository
{
    public ConcernRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<Concern?> FindByNameAsync(string name, CancellationToken token = default) =>
        await Context.Concerns.AsNoTracking()
            .SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);
}
