using Cts.AppServices.ErrorLogging;
using Cts.Domain.Entities.Complaints;
using GaEpd.EmailService;
using GaEpd.EmailService.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Web;

namespace Cts.AppServices.Notifications;

public class NotificationService(
    IEmailService emailService,
    IEmailLogRepository repository,
    IHostEnvironment environment,
    IConfiguration configuration,
    IErrorLogger errorLogger) : INotificationService
{
    private const string FailurePrefix = "Notification email not sent:";

    public async Task<NotificationResult> SendNotificationAsync(Template template, string recipientEmail,
        Complaint complaint, string? baseUrl, string? comment = null, CancellationToken token = default)
    {
        var subjectPrefix = environment.EnvironmentName switch
        {
            "Development" => "[CTS-DEV]",
            "Staging" => "[CTS-UAT]",
            _ => "[CTS]",
        };

        baseUrl ??= string.Empty;
        var complaintUrl = $"{baseUrl}Staff/Complaints/Details/{complaint.Id}";
        var subject = string.Format($"{subjectPrefix} {template.Subject}", complaint.Id.ToString());
        var textBody = string.Format(template.TextBody + Template.TextSignature, complaint.Id.ToString(),
            complaintUrl, baseUrl, complaint.CurrentOffice.Name, comment);
        var htmlBody = string.Format(template.HtmlBody + Template.HtmlSignature, complaint.Id.ToString(),
            complaintUrl, baseUrl, complaint.CurrentOffice.Name, HttpUtility.HtmlEncode(comment));

        var settings = new EmailServiceSettings();
        configuration.GetSection(nameof(EmailServiceSettings)).Bind(settings);

        if (string.IsNullOrEmpty(recipientEmail))
            return NotificationResult.FailureResult($"{FailurePrefix} A recipient could not be determined.");

        Message message;
        try
        {
            message = Message.Create(subject, recipientEmail, settings.DefaultSender, textBody, htmlBody);
        }
        catch (Exception e)
        {
            await errorLogger.LogErrorAsync(e, subject).ConfigureAwait(false);
            return NotificationResult.FailureResult($"{FailurePrefix} An error occurred when generating the email.");
        }

        if (settings.SaveEmail) await repository.InsertAsync(EmailLog.Create(message), token).ConfigureAwait(false);

        if (!settings.EnableEmail)
        {
            return NotificationResult.FailureResult($"{FailurePrefix} Emailing is not enabled on the server.");
        }

        try
        {
            await emailService.SendEmailAsync(message, settings, token).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            await errorLogger.LogErrorAsync(e, subject).ConfigureAwait(false);
            return NotificationResult.FailureResult($"{FailurePrefix} An error occurred when sending the email.");
        }

        return NotificationResult.SuccessResult();
    }
}
