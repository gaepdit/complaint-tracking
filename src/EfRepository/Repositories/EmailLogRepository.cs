using GaEpd.EmailService;
using GaEpd.EmailService.EmailLogRepository;

namespace Cts.EfRepository.Repositories;

public sealed class EmailLogRepository(AppDbContext context) : IEmailLogRepository
{
    public async Task InsertAsync(Message message, CancellationToken token = default)
    {
        var emailLog = EmailLog.Create(message);
        await context.EmailLogs.AddAsync(emailLog, token).ConfigureAwait(false);
        await context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    void IDisposable.Dispose() => context.Dispose();
    async ValueTask IAsyncDisposable.DisposeAsync() => await context.DisposeAsync().ConfigureAwait(false);

    #endregion
}
