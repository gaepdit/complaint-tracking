using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.ComplaintActions;

namespace Cts.Domain.Entities.Complaints;

public class ComplaintManager(IComplaintRepository repository) : IComplaintManager
{
    public Complaint Create(string? createdById)
    {
        var item = new Complaint(repository.GetNextId());
        item.SetCreator(createdById);
        return item;
    }

    public ComplaintAction AddAction(Complaint complaint, ActionType actionType, string? createdById)
    {
        var item = new ComplaintAction(Guid.NewGuid(), complaint, actionType);
        item.SetCreator(createdById);
        return item;
    }
}
