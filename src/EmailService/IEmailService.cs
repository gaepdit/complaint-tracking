namespace GaEpd.EmailService;

public interface IEmailService
{
    Task SendEmailAsync(Message message, EmailServiceSettings settings, CancellationToken token = default);
}
