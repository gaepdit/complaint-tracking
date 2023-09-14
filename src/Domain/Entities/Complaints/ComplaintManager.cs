namespace Cts.Domain.Entities.Complaints;

/// <inheritdoc />
public class ComplaintManager : IComplaintManager
{
    private readonly IComplaintRepository _repository;

    public ComplaintManager(IComplaintRepository repository) => _repository = repository;

    public async Task<Complaint> CreateNewComplaintAsync(string? createdById)
    {
        var item = new Complaint(await _repository.GetNextIdAsync());
        item.SetCreator(createdById);
        return item;
    }
}
