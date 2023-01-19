using Cts.Domain.Complaints;
using Cts.EfRepository.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cts.EfRepository.Repositories;

public class ComplaintRepository : BaseRepository<Complaint, int>, IComplaintRepository
{
    public ComplaintRepository(AppDbContext context) : base(context) { }

    public Task<bool> ExistsAsync(Expression<Func<Complaint, bool>> predicate, CancellationToken token = default) =>
        Context.Set<Complaint>().AsNoTracking().AnyAsync(predicate, token);
}
