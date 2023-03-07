using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints.Dto;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Http;

namespace Cts.AppServices.Complaints;

public interface IComplaintAppService : IDisposable
{
    // Public methods

    Task<ComplaintPublicViewDto?> GetPublicAsync(int id, CancellationToken token = default);

    Task<bool> PublicExistsAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<ComplaintSearchResultDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec, PaginatedRequest paging, CancellationToken token = default);

    Task<AttachmentPublicViewDto?> GetPublicAttachmentAsync(Guid id, CancellationToken token = default);

    // Admin methods

    Task<ComplaintViewDto?> GetAsync(int id, CancellationToken token = default);

    Task<bool> ExistsAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<ComplaintSearchResultDto>> SearchAsync(
        ComplaintSearchDto spec, PaginatedRequest paging, CancellationToken token = default);

    Task<AttachmentViewDto?> GetAttachmentAsync(Guid id, CancellationToken token = default);

    Task<int> CreateAsync(ComplaintCreateDto resource, CancellationToken token = default);

    Task SaveAttachmentAsync(IFormFile file, CancellationToken token = default);
}
