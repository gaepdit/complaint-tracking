namespace GaEpd.EmailService;

public class EmailServiceSettings
{
    public bool EnableEmail { get; init; }
    public bool SaveEmail { get; init; }
    public string SmtpHost { get; init; } = string.Empty;
    public int SmtpPort { get; init; }
    public string DefaultSender { get; init; } = string.Empty;
}
