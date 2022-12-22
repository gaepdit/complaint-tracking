using System.Linq.Expressions;

namespace Cts.Domain.Complaints;

public interface IComplaintRepository : IRepository<Complaint, int>
{
    // TODO: AppLibraryExtra
    public Task<int> GetCountAsync(Expression<Func<Complaint, bool>> predicate, CancellationToken token = default);
    public Task<bool> ExistsAsync(Expression<Func<Complaint, bool>> predicate, CancellationToken token = default);
}
