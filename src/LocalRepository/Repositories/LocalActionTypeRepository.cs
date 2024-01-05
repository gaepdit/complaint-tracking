using Cts.Domain.Entities.ActionTypes;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalActionTypeRepository()
    : NamedEntityRepository<ActionType>(ActionTypeData.GetActionTypes), IActionTypeRepository;
