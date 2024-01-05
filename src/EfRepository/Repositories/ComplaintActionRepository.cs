using Cts.Domain.Entities.ComplaintActions;

namespace Cts.EfRepository.Repositories;

public sealed class ComplaintActionRepository(AppDbContext dbContext) 
    : BaseRepository<ComplaintAction, Guid, AppDbContext>(dbContext), IComplaintActionRepository;
