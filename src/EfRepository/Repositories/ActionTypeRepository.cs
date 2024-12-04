using Cts.Domain.Entities.ActionTypes;

namespace Cts.EfRepository.Repositories;

public sealed class ActionTypeRepository(AppDbContext context) :
    NamedEntityRepository<ActionType, AppDbContext>(context), IActionTypeRepository;
