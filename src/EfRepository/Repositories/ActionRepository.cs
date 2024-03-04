using Cts.Domain.Entities.ComplaintActions;

namespace Cts.EfRepository.Repositories;

public sealed class ActionRepository(AppDbContext dbContext) 
    : BaseRepository<ComplaintAction, Guid, AppDbContext>(dbContext), IActionRepository;
