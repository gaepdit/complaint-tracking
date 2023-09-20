using Cts.Domain.Entities.ActionTypes;

namespace Cts.EfRepository.Repositories;

public sealed class ActionTypeRepository : NamedEntityRepository<ActionType, AppDbContext>, IActionTypeRepository
{
    public ActionTypeRepository(AppDbContext dbContext) : base(dbContext) { }
}
