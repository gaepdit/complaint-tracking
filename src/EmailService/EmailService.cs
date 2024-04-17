using GaEpd.EmailService.Utilities;
using MailKit.Net.Smtp;
using MimeKit;

namespace GaEpd.EmailService;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(Message message, EmailServiceSettings settings, CancellationToken token = default)
    {
        if (settings is { EnableEmail: false, EnableEmailAuditing: false }) return;

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(string.Empty, message.Sender));
        emailMessage.Cc.AddRange(message.CopyRecipients.Select(address => new MailboxAddress(string.Empty, address)));
        emailMessage.Subject = message.Subject;

        if (settings.EnableEmail && message.Recipients.Count > 0)
        {
            emailMessage.To.AddRange(message.Recipients.Select(address => new MailboxAddress(string.Empty, address)));

            var builder = new BodyBuilder
            {
                TextBody = message.TextBody,
                HtmlBody = message.HtmlBody
            };
            emailMessage.Body = builder.ToMessageBody();

            await SendEmailMessageAsync(emailMessage, settings, token);
        }

        if (settings is { EnableEmailAuditing: true, AuditEmailRecipients.Count: > 0 })
        {
            emailMessage.Cc.Clear();
            emailMessage.To.Clear();
            emailMessage.To.AddRange(settings.AuditEmailRecipients
                .Select(address => new MailboxAddress(string.Empty, address)));

            var auditBuilder = new BodyBuilder
            {
                TextBody = string.Concat($"Original recipient: {message.Recipients.ConcatWithSeparator(", ")}",
                    Environment.NewLine, Environment.NewLine, message.TextBody),
                HtmlBody = string.Concat($"<em>Original recipient: {message.Recipients.ConcatWithSeparator(", ")}</em>",
                    "<br><br>", message.HtmlBody)
            };
            emailMessage.Body = auditBuilder.ToMessageBody();

            await SendEmailMessageAsync(emailMessage, settings, token);
        }
    }

    private static async Task SendEmailMessageAsync(MimeMessage emailMessage, EmailServiceSettings settings,
        CancellationToken token)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(settings.SmtpHost, settings.SmtpPort, cancellationToken: token)
            .ConfigureAwait(false);
        await client.SendAsync(emailMessage, token).ConfigureAwait(false);
        await client.DisconnectAsync(true, token).ConfigureAwait(false);
    }
}
