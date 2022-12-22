using Cts.AppServices.Complaints.Dto;
using GaEpd.AppLibrary.Pagination;

namespace Cts.AppServices.Complaints;

public interface IComplaintAppService : IDisposable
{
    Task<ComplaintPublicViewDto?> GetPublicViewAsync(int id, CancellationToken token = default);

    Task<bool> PublicComplaintExistsAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<ComplaintPublicViewDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec,
        PaginatedRequest paging,
        CancellationToken token = default);
}
