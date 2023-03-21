namespace Cts.Domain.Complaints;

public interface IComplaintManager
{
    public Task SetIdAsync(Complaint complaint);
}
