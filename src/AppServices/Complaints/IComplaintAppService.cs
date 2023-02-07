using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints.Dto;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Http;

namespace Cts.AppServices.Complaints;

public interface IComplaintAppService : IDisposable
{
    Task<ComplaintPublicViewDto?> GetPublicViewAsync(int id, CancellationToken token = default);

    Task<bool> PublicComplaintExistsAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<ComplaintPublicViewDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec,
        PaginatedRequest paging,
        CancellationToken token = default);

    Task<AttachmentPublicViewDto?> GetPublicAttachmentAsync(Guid id, CancellationToken token = default);

    Task SaveAttachmentAsync(IFormFile file, CancellationToken token = default);
}
