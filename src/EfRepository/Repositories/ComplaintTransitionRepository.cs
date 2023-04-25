using Cts.Domain.Entities.ComplaintTransitions;
using Cts.EfRepository.Contexts;

namespace Cts.EfRepository.Repositories;

/// <inheritdoc cref="IComplaintTransitionRepository" />
public sealed class ComplaintTransitionRepository : 
    BaseRepository<ComplaintTransition, Guid>,
    IComplaintTransitionRepository
{
    public ComplaintTransitionRepository(AppDbContext dbContext) : base(dbContext) { }
}
