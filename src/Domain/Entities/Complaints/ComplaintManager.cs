using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.Complaints;

public class ComplaintManager(IComplaintRepository repository) : IComplaintManager
{
    public Complaint Create(ApplicationUser? user)
    {
        var item = new Complaint(repository.GetNextId()) { EnteredBy = user };
        item.SetCreator(user?.Id);
        return item;
    }

    public ComplaintAction CreateAction(Complaint complaint, ActionType actionType, ApplicationUser? user)
    {
        var action = new ComplaintAction(Guid.NewGuid(), complaint, actionType) { EnteredBy = user };
        action.SetCreator(user?.Id);
        return action;
    }

    public ComplaintTransition CreateTransition(Complaint complaint, TransitionType type, ApplicationUser? user)
    {
        var item = new ComplaintTransition(Guid.NewGuid(), complaint, type, user);
        item.SetCreator(user?.Id);

        switch (type)
        {
            case TransitionType.New:
                item.TransferredToOffice = complaint.CurrentOffice;
                break;

            case TransitionType.Assigned:
                item.TransferredToUser = complaint.CurrentOwner;
                item.TransferredToOffice = complaint.CurrentOffice;
                break;

            case TransitionType.SubmittedForReview:
                // TODO
                break;

            case TransitionType.ReturnedByReviewer:
                // TODO
                break;

            case TransitionType.Closed:
                // TODO
                break;

            case TransitionType.Reopened:
                // TODO
                break;

            case TransitionType.Deleted:
                // TODO
                break;

            case TransitionType.Restored:
                // TODO
                break;

            case TransitionType.Accepted:
                // No additional data.
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return item;
    }
}
