using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.Complaints;

public class ComplaintManager(IComplaintRepository repository) : IComplaintManager
{
    public Complaint Create( ApplicationUser? user)
    {
        var item = new Complaint(repository.GetNextId()) { EnteredBy = user };
        item.SetCreator(user?.Id);
        return item;
    }

    public ComplaintAction AddAction(Complaint complaint, ActionType actionType, ApplicationUser? user)
    {
        var action = new ComplaintAction(Guid.NewGuid(), complaint, actionType) { EnteredBy = user };
        action.SetCreator(user?.Id);
        return action;
    }
}
