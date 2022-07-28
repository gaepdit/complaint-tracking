using GaEpd.Library.Domain.Repositories;

namespace Cts.Domain.ActionTypes;

public interface IActionTypeRepository : IRepository<ActionType, Guid>
{
    Task<ActionType> FindByName(string name);
}
