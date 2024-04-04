using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Notifications;
using GaEpd.AppLibrary.Pagination;

namespace Cts.AppServices.Complaints;

public interface IComplaintService : IDisposable, IAsyncDisposable
{
    // Public read methods

    Task<ComplaintPublicViewDto?> FindPublicAsync(int id, CancellationToken token = default);

    Task<bool> PublicExistsAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<ComplaintSearchResultDto>> PublicSearchAsync(ComplaintPublicSearchDto spec,
        PaginatedRequest paging, CancellationToken token = default);

    // Staff read methods

    Task<ComplaintViewDto?> FindAsync(int id, bool includeDeletedActions = false, CancellationToken token = default);

    Task<ComplaintUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default);

    Task<bool> ExistsAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<ComplaintSearchResultDto>> SearchAsync(ComplaintSearchDto spec, PaginatedRequest paging,
        CancellationToken token = default);

    // Staff Dashboard
    Task<IReadOnlyCollection<ComplaintSearchResultDto>> GetNewComplaintsForUserAsync(string userId,
        CancellationToken token = default);

    Task<IReadOnlyCollection<ComplaintSearchResultDto>> GetOpenComplaintsForUserAsync(string userId,
        CancellationToken token = default);

    // Manager Dashboard
    Task<IReadOnlyCollection<ComplaintSearchResultDto>> GetReviewPendingComplaintsForUserAsync(string userId,
        CancellationToken token = default);

    Task<IReadOnlyCollection<ComplaintSearchResultDto>> GetUnacceptedComplaintsForOfficeAsync(Guid officeId,
        CancellationToken token = default);

    Task<IReadOnlyCollection<ComplaintSearchResultDto>> GetUnassignedComplaintsForOfficeAsync(Guid officeId,
        CancellationToken token = default);

    // Staff complaint write methods

    Task<ComplaintCreateResult> CreateAsync(ComplaintCreateDto resource,
        IAttachmentService.AttachmentServiceConfig config, string? baseUrl, CancellationToken token = default);

    Task UpdateAsync(int id, ComplaintUpdateDto resource, CancellationToken token = default);

    // Complaint transitions

    Task AcceptAsync(int id, CancellationToken token = default);

    Task<ComplaintAssignResult> AssignAsync(ComplaintAssignmentDto resource, ComplaintViewDto currentComplaint,
        string? baseUrl, CancellationToken token = default);

    Task<NotificationResult> CloseAsync(ComplaintClosureDto resource, string? baseUrl, CancellationToken token = default);
    Task<NotificationResult> ReopenAsync(ComplaintClosureDto resource, string? baseUrl, CancellationToken token = default);

    Task<NotificationResult> RequestReviewAsync(ComplaintRequestReviewDto resource, string? baseUrl,
        CancellationToken token = default);

    Task<NotificationResult> ReturnAsync(ComplaintAssignmentDto resource, string? baseUrl,
        CancellationToken token = default);
    Task DeleteAsync(ComplaintClosureDto resource, CancellationToken token = default);
    Task RestoreAsync(ComplaintClosureDto resource, CancellationToken token = default);
}
