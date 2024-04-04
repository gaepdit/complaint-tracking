namespace GaEpd.EmailService;

public record Message
{
    public string Sender { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public List<string> Recipients { get; } = [];
    public List<string> CopyRecipients { get; } = [];
    public string? TextBody { get; private set; }
    public string? HtmlBody { get; private set; }

    public static Message Create(string subject, string recipient, string sender, string? textBody, string? htmlBody,
        IEnumerable<string>? copyRecipients = null) =>
        Create(subject, [recipient], sender, textBody, htmlBody, copyRecipients);

    public static Message Create(string subject, ICollection<string> recipients, string sender, string? textBody,
        string? htmlBody, IEnumerable<string>? copyRecipients = null)
    {
        if(string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("A subject must be provided.", nameof(subject));

        if (recipients.Count == 0)
            throw new ArgumentException("At least one recipient must be specified.", nameof(recipients));

        if (recipients.Any(string.IsNullOrEmpty))
            throw new ArgumentException("Recipient cannot be null, empty, or white space.", nameof(recipients));

        if (string.IsNullOrEmpty(htmlBody) && string.IsNullOrEmpty(textBody))
            throw new ArgumentException("Either a plaintext or HTML body must be provided.", nameof(htmlBody));

        var message = new Message
        {
            Sender = sender,
            Subject = subject,
            TextBody = textBody,
            HtmlBody = htmlBody,
        };

        message.Recipients.AddRange(recipients);
        if (copyRecipients != null) message.CopyRecipients.AddRange(copyRecipients);

        return message;
    }
}
