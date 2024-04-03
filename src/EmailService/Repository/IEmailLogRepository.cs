namespace GaEpd.EmailService.Repository;

public interface IEmailLogRepository : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Inserts a new <see cref="EmailLog"/>.
    /// </summary>
    /// <param name="emailLog">The Email Log to insert.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task InsertAsync(EmailLog emailLog, CancellationToken token = default);
}
