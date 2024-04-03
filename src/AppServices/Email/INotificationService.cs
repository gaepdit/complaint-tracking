using Cts.Domain.Entities.Complaints;
using GaEpd.EmailService;

namespace Cts.AppServices.Email;

public interface INotificationService
{
    Task<OperationResult> SendNotificationAsync(EmailTemplate template, string recipient, Complaint complaint,
        string complaintUrl, CancellationToken token = default);
}
