using Cts.Domain.Entities.ComplaintTransitions;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalComplaintTransitionRepository()
    : BaseRepository<ComplaintTransition, Guid>(ComplaintTransitionData.GetComplaintTransitions),
        IComplaintTransitionRepository;
