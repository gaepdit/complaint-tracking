using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints.Dto;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Http;

namespace Cts.AppServices.Complaints;

public interface IComplaintService : IDisposable, IAsyncDisposable
{
    // Public methods

    Task<ComplaintPublicViewDto?> FindPublicAsync(int id, CancellationToken token = default);

    Task<bool> PublicExistsAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<ComplaintSearchResultDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec, PaginatedRequest paging, CancellationToken token = default);

    Task<AttachmentPublicViewDto?> FindPublicAttachmentAsync(Guid id, CancellationToken token = default);

    // Staff read methods

    Task<ComplaintViewDto?> FindAsync(int id, bool includeDeletedActions = false, CancellationToken token = default);

    Task<ComplaintUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default);

    Task<bool> ExistsAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<ComplaintSearchResultDto>> SearchAsync(
        ComplaintSearchDto spec, PaginatedRequest paging, CancellationToken token = default);

    Task<AttachmentViewDto?> FindAttachmentAsync(Guid id, CancellationToken token = default);

    // Staff write methods

    Task<int> CreateAsync(ComplaintCreateDto resource, CancellationToken token = default);

    Task UpdateAsync(int id, ComplaintUpdateDto resource, CancellationToken token = default);

    Task SaveAttachmentAsync(IFormFile file, CancellationToken token = default);
}
