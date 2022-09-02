using Cts.Domain.ActionTypes;
using Cts.Domain.Entities;
using Cts.TestData.ActionTypes;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalActionTypeRepository : BaseRepository<ActionType, Guid>, IActionTypeRepository
{
    public LocalActionTypeRepository() : base(ActionTypeData.GetActionTypes) { }

    public Task<ActionType?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => e.Name == name));
}
