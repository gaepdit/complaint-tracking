using GaEpd.EmailService.Repository;

namespace Cts.EfRepository.Repositories;

public sealed class EmailLogRepository(AppDbContext dbContext) : IEmailLogRepository
{
    public async Task InsertAsync(EmailLog emailLog, CancellationToken token = default)
    {
        await dbContext.EmailLogs.AddAsync(emailLog, token).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public void Dispose() => dbContext.Dispose();
    public ValueTask DisposeAsync() => dbContext.DisposeAsync();
}
