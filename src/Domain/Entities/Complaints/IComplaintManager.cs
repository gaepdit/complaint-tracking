namespace Cts.Domain.Entities.Complaints;

public interface IComplaintManager
{
    public Task<Complaint> CreateNewComplaintAsync();
}
