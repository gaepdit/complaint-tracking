using Cts.Domain.Complaints;
using Cts.Infrastructure.Contexts;

namespace Cts.Infrastructure.Repositories;

public class ComplaintRepository : BaseRepository<Complaint, int>, IComplaintRepository
{
    public ComplaintRepository(AppDbContext context) : base(context) { }
}
