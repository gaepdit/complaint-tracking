using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.Complaints;

public interface IComplaintManager
{
    /// <summary>
    /// Creates a new <see cref="Complaint"/>.
    /// </summary>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The Complaint that was created.</returns>
    public Complaint Create(ApplicationUser? user);

    /// <summary>
    /// Creates a new <see cref="ComplaintAction"/>.
    /// </summary>
    /// <param name="complaint">The <see cref="Complaint"/> this Action belongs to.</param>
    /// <param name="actionType">The <see cref="ActionType"/> of this Action.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The Complaint ActionItem that was created.</returns>
    ComplaintAction AddAction(Complaint complaint, ActionType actionType, ApplicationUser? user);
}
