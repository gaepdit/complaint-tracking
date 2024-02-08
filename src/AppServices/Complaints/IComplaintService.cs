using Cts.AppServices.Complaints.Dto;
using GaEpd.AppLibrary.Pagination;

namespace Cts.AppServices.Complaints;

public interface IComplaintService : IDisposable, IAsyncDisposable
{
    // Public read methods

    Task<ComplaintPublicViewDto?> FindPublicAsync(int id, CancellationToken token = default);

    Task<bool> PublicExistsAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<ComplaintSearchResultDto>> PublicSearchAsync(
        ComplaintPublicSearchDto spec, PaginatedRequest paging, CancellationToken token = default);

    // Staff read methods

    Task<ComplaintViewDto?> FindAsync(int id, bool includeDeletedActions = false, CancellationToken token = default);

    Task<ComplaintUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default);

    Task<bool> ExistsAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<ComplaintSearchResultDto>> SearchAsync(
        ComplaintSearchDto spec, PaginatedRequest paging, CancellationToken token = default);

    // Staff write methods

    Task<int> CreateAsync(ComplaintCreateDto resource, CancellationToken token = default);

    Task UpdateAsync(int id, ComplaintUpdateDto resource, CancellationToken token = default);


    // Management write methods
    Task DeleteAsync(int complaintId, CancellationToken token = default);
    Task RestoreAsync(int complaintId, CancellationToken token = default);
}
