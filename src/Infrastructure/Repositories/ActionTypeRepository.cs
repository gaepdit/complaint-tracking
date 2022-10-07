using Cts.Domain.ActionTypes;
using Cts.Domain.Entities;
using Cts.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cts.Infrastructure.Repositories;

public sealed class ActionTypeRepository : BaseRepository<ActionType, Guid>, IActionTypeRepository
{
    public ActionTypeRepository(CtsDbContext dbContext) : base(dbContext) { }

    public Task<ActionType?> FindByNameAsync(string name, CancellationToken token = default) =>
        Context.ActionTypes.AsNoTracking().SingleOrDefaultAsync(e => e.Name == name, token);
}
