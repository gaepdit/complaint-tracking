﻿using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints.Dto.Command;
using Cts.AppServices.Complaints.Dto.Query;
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

    // Staff complaint write methods

    Task<ComplaintCreateResult> CreateAsync(ComplaintCreateDto resource,
        IAttachmentService.AttachmentServiceConfig config, CancellationToken token = default);

    Task UpdateAsync(int id, ComplaintUpdateDto resource, CancellationToken token = default);

    // Complaint transitions

    Task AcceptAsync(int id, CancellationToken token = default);
    Task AssignAsync(ComplaintAssignDto resource, CancellationToken token = default);
    Task CloseAsync(ComplaintClosureDto resource, CancellationToken token = default);
    Task ReopenAsync(ComplaintClosureDto resource, CancellationToken token = default);
    Task RequestReviewAsync(ComplaintRequestReviewDto resource, CancellationToken token = default);
    Task ReturnAsync(ComplaintAssignDto resource, CancellationToken token = default);

    // Management complaint write methods

    Task DeleteAsync(int complaintId, CancellationToken token = default);
    Task RestoreAsync(int complaintId, CancellationToken token = default);
}
