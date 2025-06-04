using Cts.AppServices.Notifications;
using GaEpd.EmailService;

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
