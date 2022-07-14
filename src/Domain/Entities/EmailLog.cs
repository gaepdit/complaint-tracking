namespace Cts.Domain.Entities;

public class EmailLog
{
    public Guid Id { get; set; }
    public DateTime DateSent { get; set; }
    public string? To { get; set; }
    public string? From { get; set; }
    public string? Subject { get; set; }
    public string? TextBody { get; set; }
    public string? HtmlBody { get; set; }
}
