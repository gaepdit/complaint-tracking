using GaEpd.EmailService;
using GaEpd.EmailService.EmailLogRepository;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalEmailLogRepository : IEmailLogRepository
{
    public Task InsertAsync(Message message, CancellationToken token = default) => Task.CompletedTask;

    public void Dispose()
    {
        // Method intentionally left empty.
    }

    public ValueTask DisposeAsync() => default;
}
