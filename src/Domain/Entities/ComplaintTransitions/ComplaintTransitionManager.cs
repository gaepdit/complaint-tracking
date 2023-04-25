using Cts.Domain.Entities.Complaints;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.ComplaintTransitions;

/// <inheritdoc />
public class ComplaintTransitionManager : IComplaintTransitionManager
{
    public ComplaintTransition Create(Complaint complaint, TransitionType type, ApplicationUser? user)
    {
        var transition = new ComplaintTransition(Guid.NewGuid(), complaint, type, user);

        switch (type)
        {
            case TransitionType.New:
                transition.TransferredToOffice = complaint.CurrentOffice;
                break;

            case TransitionType.Assigned:
                transition.TransferredToUser = complaint.CurrentOwner;
                transition.TransferredToOffice = complaint.CurrentOffice;
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

        return transition;
    }
}
