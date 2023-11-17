using Cts.Domain.Entities.Attachments;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalAttachmentRepository() 
    : BaseRepository<Attachment, Guid>(AttachmentData.GetAttachments), IAttachmentRepository;
