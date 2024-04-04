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
        Sender = message.Sender,
        Subject = message.Subject,
        Recipients = message.Recipients.ConcatWithSeparator(","),
        CopyRecipients = message.CopyRecipients.ConcatWithSeparator(","),
        TextBody = message.TextBody,
        HtmlBody = message.HtmlBody,
        CreatedAt = DateTimeOffset.Now,
    };
}

public static class StringExtensions
{
    public static string ConcatWithSeparator(this IEnumerable<string?> items, string separator = " ") =>
        string.Join(separator, items.Where(s => !string.IsNullOrEmpty(s)));
}
