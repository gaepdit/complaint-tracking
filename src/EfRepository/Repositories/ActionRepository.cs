using Cts.Domain.Entities.ComplaintActions;

namespace Cts.EfRepository.Repositories;

public sealed class ActionRepository(AppDbContext context)
    : BaseRepository<ComplaintAction, Guid, AppDbContext>(context), IActionRepository;
