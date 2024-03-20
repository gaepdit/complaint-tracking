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

    public void Assign(Complaint complaint, Office office, ApplicationUser? owner, ApplicationUser? user)
    {
        complaint.SetUpdater(user?.Id);
        complaint.CurrentOffice = office;
        complaint.CurrentOwner = owner;
        complaint.CurrentOwnerAssignedDate = owner == null ? null : DateTimeOffset.Now;

        if (owner != null && owner == user)
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

    public void RequestReview(Complaint complaint, ApplicationUser reviewer, ApplicationUser? user)
    {
        complaint.SetUpdater(user?.Id);
        complaint.Status = ComplaintStatus.ReviewPending;
        complaint.ReviewedBy = reviewer;
    }

    public void Return(Complaint complaint, Office office, ApplicationUser? owner, ApplicationUser? user)
    {
        complaint.SetUpdater(user?.Id);
        complaint.Status = ComplaintStatus.UnderInvestigation;
        complaint.ReviewedBy = null;
        complaint.ReviewComments = null;
        complaint.CurrentOffice = office;
        complaint.CurrentOwner = owner;
        complaint.CurrentOwnerAssignedDate = owner == null ? null : DateTimeOffset.Now;
        complaint.CurrentOwnerAcceptedDate = owner != null && owner == user ? DateTimeOffset.Now : null;
    }

    public void Delete(Complaint complaint, string? comment, ApplicationUser? user)
    {
        complaint.SetDeleted(user?.Id);
        complaint.DeleteComments = comment;
        complaint.DeletedBy = user;
    }

    public void Restore(Complaint complaint, ApplicationUser? user)
    {
        complaint.SetNotDeleted();
        complaint.DeleteComments = null;
    }

    public ComplaintTransition CreateTransition(Complaint complaint, TransitionType type, ApplicationUser? user,
        string? comment)
    {
        var transition = new ComplaintTransition(Guid.NewGuid(), complaint, type, user);
        transition.SetCreator(user?.Id);
        transition.Comment = comment;

        switch (type)
        {
            case TransitionType.New:
                transition.TransferredToOffice = complaint.CurrentOffice;
                break;

            case TransitionType.Assigned:
            case TransitionType.Reopened:
            case TransitionType.ReturnedByReviewer:
                transition.TransferredToUser = complaint.CurrentOwner;
                transition.TransferredToOffice = complaint.CurrentOffice;
                break;

            case TransitionType.SubmittedForReview:
                transition.TransferredToUser = complaint.ReviewedBy;
                transition.TransferredToOffice = complaint.CurrentOffice;
                break;

            case TransitionType.Accepted:
            case TransitionType.Closed:
            case TransitionType.Deleted:
            case TransitionType.Restored:
                // No additional data changed.
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return transition;
    }
}
