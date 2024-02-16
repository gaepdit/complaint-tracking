using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.Complaints;

public class ComplaintManager(IComplaintRepository repository) : IComplaintManager
{
    // Complaints
    public Complaint Create(ApplicationUser? user)
    {
        var item = new Complaint(repository.GetNextId()) { EnteredBy = user };
        item.SetCreator(user?.Id);
        return item;
    }

    // Complaint actions
    public ComplaintAction CreateAction(Complaint complaint, ActionType actionType, ApplicationUser? user)
    {
        var action = new ComplaintAction(Guid.NewGuid(), complaint, actionType) { EnteredBy = user };
        action.SetCreator(user?.Id);
        return action;
    }

    // Complaint transitions

    public void Accept(Complaint complaint, ApplicationUser? user)
    {
        complaint.SetUpdater(user?.Id);
        complaint.Status = ComplaintStatus.UnderInvestigation;
        complaint.CurrentOwnerAcceptedDate = DateTimeOffset.Now;
    }

    public void Assign(Complaint complaint, Office office, ApplicationUser? owner, string? comment,
        ApplicationUser? user)
    {
        complaint.SetUpdater(user?.Id);
        complaint.CurrentOffice = office;
        complaint.CurrentOwner = owner;
        if (owner is null)
        {
            complaint.CurrentOwnerAssignedDate = null;
            return;
        }

        complaint.CurrentOwnerAssignedDate = DateTimeOffset.Now;
        if (owner == user)
        {
            complaint.CurrentOwnerAcceptedDate = DateTimeOffset.Now;
            complaint.Status = ComplaintStatus.UnderInvestigation;
        }
        else
        {
            complaint.CurrentOwnerAcceptedDate = null;
        }
    }

    public void Close(Complaint complaint, string? comment, ApplicationUser? user)
    {
        complaint.SetUpdater(user?.Id);
        complaint.Status = ComplaintStatus.Closed;
        complaint.ComplaintClosed = true;
        complaint.ComplaintClosedDate = DateTime.Now;
        complaint.ReviewedBy = user;
        complaint.ReviewComments = comment;
    }

    public void Reopen(Complaint complaint, ApplicationUser? user)
    {
        complaint.SetUpdater(user?.Id);
        complaint.Status = ComplaintStatus.UnderInvestigation;
        complaint.ComplaintClosed = false;
        complaint.ComplaintClosedDate = null;
        complaint.ReviewedBy = null;
    }

    public ComplaintTransition CreateTransition(Complaint complaint, TransitionType type, ApplicationUser? user,
        string? comment)
    {
        var item = new ComplaintTransition(Guid.NewGuid(), complaint, type, user);
        item.SetCreator(user?.Id);
        item.Comment = comment;

        switch (type)
        {
            case TransitionType.New:
                item.TransferredToOffice = complaint.CurrentOffice;
                break;

            case TransitionType.Assigned:
            case TransitionType.Reopened:
                item.TransferredToUser = complaint.CurrentOwner;
                item.TransferredToOffice = complaint.CurrentOffice;
                break;

            case TransitionType.SubmittedForReview:
                // TODO
                break;

            case TransitionType.ReturnedByReviewer:
                // TODO
                break;


            case TransitionType.Deleted:
                // TODO
                break;

            case TransitionType.Restored:
                // TODO
                break;

            case TransitionType.Closed:
            case TransitionType.Accepted:
                // No additional data.
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return item;
    }
}
