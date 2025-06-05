using GaEpd.AppLibrary.Extensions;
using GaEpd.EmailService;
using System.ComponentModel.DataAnnotations;

namespace Cts.AppServices.Notifications;

public interface IEmailLogRepository : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Saves a copy of an <see cref="Message"/> to the configured repository.
    /// </summary>
    /// <param name="message">The Message to log.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task InsertAsync(Message message, CancellationToken token = default);
}

public record EmailLog
{
    [Key]
    public required Guid Id { get; init; }

    public DateTimeOffset? CreatedAt { get; init; }

    [StringLength(300)]
    public required string Sender { get; init; }

    [StringLength(200)]
    public required string Subject { get; init; }

    [StringLength(2000)]
    public required string Recipients { get; init; }

    [StringLength(2000)]
    public string? CopyRecipients { get; init; }

    [StringLength(15_000)]
    public string? TextBody { get; init; }

    [StringLength(20_000)]
    public string? HtmlBody { get; init; }

    public static EmailLog Create(Message message) => new()
    {
        Id = Guid.NewGuid(),
        Sender = StringExtensions.ConcatWithSeparator([message.SenderName, $"<{message.SenderEmail}>"])
            .Truncate(300),
        Subject = message.Subject.Truncate(200),
        Recipients = message.Recipients.ConcatWithSeparator(",").Truncate(2000),
        CopyRecipients = message.CopyRecipients.ConcatWithSeparator(",").Truncate(2000),
        TextBody = message.TextBody.Truncate(15_000),
        HtmlBody = message.HtmlBody.Truncate(20_000),
        CreatedAt = DateTimeOffset.Now,
    };
}
