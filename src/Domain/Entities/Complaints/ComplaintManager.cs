namespace Cts.Domain.Entities.Complaints;

public class ComplaintManager : IComplaintManager
{
    private readonly IComplaintRepository _repository;

    public ComplaintManager(IComplaintRepository repository) => _repository = repository;

    public Complaint CreateNewComplaint(string? createdById)
    {
        var item = new Complaint(_repository.GetNextId());
        item.SetCreator(createdById);
        return item;
    }
}
