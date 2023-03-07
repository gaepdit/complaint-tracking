namespace Cts.Domain.Complaints;

/// <inheritdoc />
public class ComplaintManager : IComplaintManager
{
    public Complaint Create()
    {
        return new Complaint();
    }
}
