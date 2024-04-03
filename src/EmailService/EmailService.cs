using MailKit.Net.Smtp;
using MimeKit;

namespace GaEpd.EmailService;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(Message message, EmailServiceSettings settings, CancellationToken token = default)
    {
        if (!settings.EnableEmail) return;

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(string.Empty, message.Sender));
        emailMessage.To.AddRange(message.Recipients.Select(address => new MailboxAddress(string.Empty, address)));
        emailMessage.Cc.AddRange(message.CopyRecipients.Select(address => new MailboxAddress(string.Empty, address)));
        emailMessage.Subject = message.Subject;
        var builder = new BodyBuilder { TextBody = message.TextBody, HtmlBody = message.HtmlBody };
        emailMessage.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(settings.SmtpHost, settings.SmtpPort, cancellationToken: token)
            .ConfigureAwait(false);
        await client.SendAsync(emailMessage, token).ConfigureAwait(false);
        await client.DisconnectAsync(true, token).ConfigureAwait(false);
    }
}
