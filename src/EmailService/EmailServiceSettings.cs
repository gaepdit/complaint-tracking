namespace GaEpd.EmailService;

public class EmailServiceSettings
{
    public bool EnableEmail { get; init; }
    public bool SaveEmail { get; init; }
    public string SmtpHost { get; init; } = null!;
    public int SmtpPort { get; init; }
    public string DefaultSender { get; init; } = null!;
    public string SecureSocketOption { get; init; } = null!;
    public bool EnableEmailAuditing { get; init; }
    public List<string> AuditEmailRecipients { get; init; } = null!;
}
