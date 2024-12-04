using Cts.Domain.Entities.Attachments;

namespace Cts.EfRepository.Repositories;

public sealed class AttachmentRepository(AppDbContext context)
    : BaseRepository<Attachment, Guid, AppDbContext>(context), IAttachmentRepository;
