using Cts.Domain.Entities.Complaints;
using GaEpd.EmailService;

namespace Cts.AppServices.Email;

public interface INotificationService
{
    Task<OperationResult> SendNotificationAsync(EmailTemplate template, string recipientEmail, Complaint complaint,
        string? baseUrl, string? comments = null, CancellationToken token = default);
}
