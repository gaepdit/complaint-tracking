using Cts.Domain.Entities.ComplaintTransitions;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

/// <inheritdoc cref="IComplaintTransitionRepository" />
public sealed class LocalComplaintTransitionRepository :
    BaseRepository<ComplaintTransition, Guid>, 
    IComplaintTransitionRepository
{
    public LocalComplaintTransitionRepository() : base(ComplaintTransitionData.GetComplaintTransitions) { }
}
