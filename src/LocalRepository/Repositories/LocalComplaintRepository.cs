using Cts.Domain.Complaints;
using Cts.TestData;
using System.Linq.Expressions;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalComplaintRepository : BaseRepository<Complaint, int>, IComplaintRepository
{
    public LocalComplaintRepository() : base(ComplaintData.GetComplaints) { }

    public Task<int> GetCountAsync(Expression<Func<Complaint, bool>> predicate, CancellationToken token = default) =>
        Task.FromResult(Items.Count(predicate.Compile()));

    public Task<bool> ExistsAsync(Expression<Func<Complaint, bool>> predicate, CancellationToken token = default) =>
        Task.FromResult(Items.Any(predicate.Compile()));
}
