namespace Cts.AppServices.ActionTypes;

public interface IActionTypeAppService : IDisposable
{
    Task<ActionTypeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<ActionTypeViewDto>> GetListAsync(CancellationToken token = default);
    Task<Guid> CreateAsync(string name, CancellationToken token = default);
    Task UpdateAsync(ActionTypeUpdateDto resource, CancellationToken token = default);
}
