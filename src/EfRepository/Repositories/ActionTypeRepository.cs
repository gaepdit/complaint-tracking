using Cts.Domain.Entities.ActionTypes;

namespace Cts.EfRepository.Repositories;

public sealed class ActionTypeRepository(AppDbContext dbContext) :
    NamedEntityRepository<ActionType, AppDbContext>(dbContext), IActionTypeRepository;
