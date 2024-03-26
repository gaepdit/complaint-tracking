using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.Complaints.QueryDto;
using Microsoft.AspNetCore.Http;

namespace Cts.AppServices.Attachments;

public interface IAttachmentService
{
    public const int MaxSimultaneousUploads = 10;

    // Attachment DTOs
    Task<AttachmentViewDto?> FindAttachmentAsync(Guid id, CancellationToken token = default);
    Task<AttachmentViewDto?> FindPublicAttachmentAsync(Guid id, CancellationToken token = default);
    Task<ComplaintViewDto?> FindComplaintForAttachmentAsync(Guid attachmentId, CancellationToken token = default);

    // Attachment files
    Task<byte[]> GetAttachmentFileAsync(string fileId, bool getThumbnail, AttachmentServiceConfig config,
        CancellationToken token = default);

    Task DeleteAttachmentAsync(AttachmentViewDto attachmentView, AttachmentServiceConfig config,
        CancellationToken token = default);

    Task<int> SaveAttachmentsAsync(int complaintId, List<IFormFile> files, AttachmentServiceConfig config,
        CancellationToken token = default);

    // Configuration
    public record AttachmentServiceConfig(string AttachmentsFolder, string ThumbnailsFolder, int ThumbnailSize);
}
