// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
// FUTURE: Remove the above line once email has been implemented.

namespace Cts.Domain.Entities.EmailLogs;

public class EmailLog : Entity<Guid>
{
    public DateTimeOffset SentDate { get; set; }
    public string? To { get; set; }
    public string? From { get; set; }
    public string? Subject { get; set; }
    public string? TextBody { get; set; }
    public string? HtmlBody { get; set; }
}
