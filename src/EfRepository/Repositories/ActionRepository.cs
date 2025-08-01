using Cts.Domain.Entities.ComplaintActions;

namespace Cts.EfRepository.Repositories;

public sealed class ActionRepository(AppDbContext context)
    : BaseRepositoryWithMapping<ComplaintAction, Guid, AppDbContext>(context), IActionRepository;
