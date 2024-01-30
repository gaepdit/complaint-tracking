namespace Cts.AppServices.ComplaintActions;

public interface IComplaintActionService : IDisposable, IAsyncDisposable
{
    Task<Guid> CreateAsync(ComplaintActionCreateDto resource, CancellationToken token = default);
    Task<ComplaintActionViewDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<ComplaintActionUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(Guid id, ComplaintActionUpdateDto resource, CancellationToken token = default);
    Task DeleteAsync(Guid actionItemId, CancellationToken token = default);
    Task RestoreAsync(Guid actionItemId, CancellationToken token = default);
}
