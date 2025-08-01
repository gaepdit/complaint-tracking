using Cts.Domain.Entities.ComplaintActions;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalActionRepository()
    : BaseRepositoryWithMapping<ComplaintAction, Guid>(ComplaintActionData.GetComplaintActions), IActionRepository;
