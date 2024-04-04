using Cts.Domain.Entities.Complaints;

namespace Cts.AppServices.Notifications;

public interface INotificationService
{
    Task<NotificationResult> SendNotificationAsync(Template template, string recipientEmail, Complaint complaint,
        string? baseUrl, string? comment = null, CancellationToken token = default);
}
