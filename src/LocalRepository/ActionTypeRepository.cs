using Cts.Domain.ActionTypes;
using Cts.Domain.Entities;
using static Cts.TestData.ActionTypes.Data;

namespace Cts.LocalRepository;

public sealed class ActionTypeRepository : BaseRepository<ActionType, Guid>, IActionTypeRepository
{
    public ActionTypeRepository() : base(GetActionTypes) { }

    public Task<ActionType?> FindByNameAsync(string name, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.SingleOrDefault(e => e.Name == name));
}
