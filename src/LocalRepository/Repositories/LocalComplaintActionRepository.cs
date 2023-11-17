using Cts.Domain.Entities.ComplaintActions;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalComplaintActionRepository() 
    : BaseRepository<ComplaintAction, Guid>(ComplaintActionData.GetComplaintActions), IComplaintActionRepository;
