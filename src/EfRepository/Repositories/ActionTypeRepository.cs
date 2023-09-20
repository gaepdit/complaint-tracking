using Cts.Domain.Entities.ActionTypes;

namespace Cts.EfRepository.Repositories;

public sealed class ActionTypeRepository : NamedEntityRepository<ActionType>, IActionTypeRepository
{
    public ActionTypeRepository(DbContext dbContext) : base(dbContext) { }
}
