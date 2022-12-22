using System.Linq.Expressions;

namespace Cts.Domain.Complaints;

public interface IComplaintRepository : IRepository<Complaint, int>
{
    public Task<bool> ExistsAsync(Expression<Func<Complaint, bool>> predicate, CancellationToken token = default);
}
