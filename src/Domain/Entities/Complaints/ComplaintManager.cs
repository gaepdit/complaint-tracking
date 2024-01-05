namespace Cts.Domain.Entities.Complaints;

public class ComplaintManager(IComplaintRepository repository) : IComplaintManager
{
    public Complaint CreateNewComplaint(string? createdById)
    {
        var item = new Complaint(repository.GetNextId());
        item.SetCreator(createdById);
        return item;
    }
}
