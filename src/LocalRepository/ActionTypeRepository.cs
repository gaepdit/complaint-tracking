using Cts.Domain.ActionTypes;
using static Cts.TestData.TestData;

namespace Cts.LocalRepository;

public sealed class ActionTypeRepository : Repository<ActionType, Guid>, IActionTypeRepository
{
    public ActionTypeRepository() : base(ActionTypes) { }

    public Task<ActionType?> FindByNameAsync(string name, CancellationToken cancellationToken = default) =>
        Task.FromResult(Items.SingleOrDefault(e => e.Name == name));
}
