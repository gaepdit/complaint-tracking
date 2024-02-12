using Cts.AppServices.Attachments.Dto;

namespace Cts.AppServices.Attachments;

public interface IAttachmentService
{
    Task<AttachmentViewDto?> FindAttachmentAsync(Guid id, CancellationToken token = default);
    Task<AttachmentViewDto?> FindPublicAttachmentAsync(Guid id, CancellationToken token = default);

    Task<byte[]> GetAttachmentFileAsync(string fileId, bool getThumbnail, AttachmentServiceConfig config,
        CancellationToken token = default);

    Task DeleteAttachmentAsync(AttachmentViewDto attachmentView, AttachmentServiceConfig config,
        CancellationToken token = default);

    Task SaveAttachmentsAsync(AttachmentsCreateDto resource, AttachmentServiceConfig config,
        CancellationToken token = default);

    public record AttachmentServiceConfig(string AttachmentsFolder, string ThumbnailsFolder, int ThumbnailSize);
}
