using Cts.Domain.Entities.ComplaintTransitions;

namespace Cts.EfRepository.Repositories;

public sealed class ComplaintTransitionRepository(AppDbContext context)
    : BaseRepository<ComplaintTransition, Guid, AppDbContext>(context), IComplaintTransitionRepository;
