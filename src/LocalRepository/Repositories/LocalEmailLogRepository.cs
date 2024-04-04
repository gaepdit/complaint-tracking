using GaEpd.EmailService.Repository;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalEmailLogRepository : IEmailLogRepository
{
    public Task InsertAsync(EmailLog emailLog, CancellationToken token = default) => Task.CompletedTask;

    public void Dispose()
    {
        // Method intentionally left empty.
    }

    public ValueTask DisposeAsync() => default;
}
