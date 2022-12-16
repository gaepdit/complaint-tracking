using Cts.Domain.Complaints;
using Cts.TestData;

namespace Cts.LocalRepository.Repositories;

public sealed class LocalComplaintRepository : BaseRepository<Complaint, int>, IComplaintRepository
{
    public LocalComplaintRepository() : base(ComplaintData.GetComplaints) { }
}
