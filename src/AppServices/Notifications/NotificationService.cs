using Cts.AppServices.ErrorLogging;
using Cts.Domain.Entities.Complaints;
using GaEpd.EmailService;
using GaEpd.EmailService.EmailLogRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Web;

namespace Cts.AppServices.Notifications;

public class NotificationService(
    IEmailService emailService,
    IEmailLogRepository emailLogRepository,
    IHostEnvironment environment,
    IConfiguration configuration,
    ILogger<NotificationService> logger,
    IErrorLogger errorLogger) : INotificationService
{
    internal static readonly EventId NotificationServiceFailure = new(2301, nameof(NotificationServiceFailure));
    internal static readonly EventId NotificationServiceException = new(2302, nameof(NotificationServiceException));
    internal static readonly EventId NotificationPreparationFailure = new(2303, nameof(NotificationPreparationFailure));
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
        comment = string.IsNullOrWhiteSpace(comment) ? "[No comment entered.]" : comment;
        var textBody = string.Format(template.TextBody + Template.TextSignature, complaint.Id.ToString(),
            complaintUrl, baseUrl, complaint.CurrentOffice.Name, comment);
        var htmlBody = string.Format(template.HtmlBody + Template.HtmlSignature, complaint.Id.ToString(),
            complaintUrl, baseUrl, complaint.CurrentOffice.Name, HttpUtility.HtmlEncode(comment));

        var settings = new EmailServiceSettings();
        configuration.GetSection(nameof(EmailServiceSettings)).Bind(settings);

        if (string.IsNullOrEmpty(recipientEmail))
        {
            logger.LogWarning(NotificationServiceFailure,
                "Notification email attempted with empty recipient for Complaint {ComplaintId}.", complaint.Id);
            return NotificationResult.FailureResult($"{FailurePrefix} A recipient could not be determined.");
        }

        if (settings is { EnableEmail: false, EnableEmailAuditing: false })
        {
            logger.LogWarning(NotificationServiceFailure,
                "Emailing is not enabled on the server. Notification attempted for Complaint {ComplaintId}.",
                complaint.Id);
            return NotificationResult.FailureResult($"{FailurePrefix} Emailing is not enabled on the server.");
        }

        Message message;
        try
        {
            message = Message.Create(subject, recipientEmail, textBody, htmlBody, settings.DefaultSenderName,
                settings.DefaultSenderEmail);
        }
        catch (Exception e)
        {
            await errorLogger.LogErrorAsync(e, subject).ConfigureAwait(false);
            logger.LogError(NotificationServiceException, e,
                "Exception raised sending a notification email for Complaint {ComplaintId}.", complaint.Id);
            return NotificationResult.FailureResult($"{FailurePrefix} An error occurred when generating the email.");
        }

        _ = emailService.SendEmailAsync(message, token: token);
        if (settings.EnableEmailLog)
        {
            await emailLogRepository.InsertAsync(message, token: token).ConfigureAwait(false);
        }

        return NotificationResult.SuccessResult();
    }
}
