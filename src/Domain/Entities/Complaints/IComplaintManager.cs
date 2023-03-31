namespace Cts.Domain.Entities.Complaints;

public interface IComplaintManager
{
    public Task SetIdAsync(Complaint complaint);
}
