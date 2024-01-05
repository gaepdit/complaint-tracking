using Cts.Domain.Entities.ComplaintTransitions;

namespace Cts.EfRepository.Repositories;

public sealed class ComplaintTransitionRepository(AppDbContext dbContext) 
    : BaseRepository<ComplaintTransition, Guid, AppDbContext>(dbContext), IComplaintTransitionRepository;
