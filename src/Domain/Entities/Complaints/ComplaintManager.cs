using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.Complaints;

public class ComplaintManager(IComplaintRepository repository) : IComplaintManager
{
    public Complaint Create(string? createdById)
    {
        var item = new Complaint(repository.GetNextId());
        item.SetCreator(createdById);
        return item;
    }

    public ComplaintAction AddAction(Complaint complaint, ActionType actionType, ApplicationUser? user)
    {
        var action = new ComplaintAction(Guid.NewGuid(), complaint, actionType);
        action.SetCreator(user?.Id);
        action.SetEnteredBy(user);
        return action;
    }
}
