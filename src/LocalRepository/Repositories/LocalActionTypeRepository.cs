using Cts.Domain.Entities.ActionTypes;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalActionTypeRepository : BaseRepository<ActionType, Guid>, IActionTypeRepository
{
    public LocalActionTypeRepository() : base(ActionTypeData.GetActionTypes) { }

    public Task<ActionType?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => string.Equals(e.Name.ToUpper(), name.ToUpper())));
}
