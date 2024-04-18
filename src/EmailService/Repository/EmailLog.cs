using GaEpd.EmailService.Utilities;
using System.ComponentModel.DataAnnotations;

namespace GaEpd.EmailService.Repository;

public record EmailLog
{
    [Key]
    public required Guid Id { get; init; }

    public DateTimeOffset? CreatedAt { get; init; }

    [StringLength(200)]
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
        Sender = message.Sender.Truncate(200),
        Subject = message.Subject.Truncate(200),
        Recipients = message.Recipients.ConcatWithSeparator(",").Truncate(2000),
        CopyRecipients = message.CopyRecipients.ConcatWithSeparator(",").Truncate(2000),
        TextBody = message.TextBody.Truncate(15_000),
        HtmlBody = message.HtmlBody.Truncate(20_000),
        CreatedAt = DateTimeOffset.Now,
    };
}
