using Cts.Domain.ActionTypes;
using Cts.EfRepository.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cts.EfRepository.Repositories;

public sealed class ActionTypeRepository : BaseRepository<ActionType, Guid>, IActionTypeRepository
{
    public ActionTypeRepository(AppDbContext dbContext) : base(dbContext) { }

    public Task<ActionType?> FindByNameAsync(string name, CancellationToken token = default) =>
        Context.ActionTypes.AsNoTracking()
            .SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);
}
