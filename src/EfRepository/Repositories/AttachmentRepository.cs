using Cts.Domain.Entities.Attachments;

namespace Cts.EfRepository.Repositories;

public sealed class AttachmentRepository(AppDbContext dbContext)
    : BaseRepository<Attachment, Guid, AppDbContext>(dbContext), IAttachmentRepository;
