using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.ComplaintActions;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.Complaints;

public interface IComplaintManager
{
    /// <summary>
    /// Creates a new <see cref="Complaint"/>.
    /// </summary>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The Complaint that was created.</returns>
    Complaint Create(ApplicationUser? user);

    /// <summary>
    /// Creates a new <see cref="ComplaintAction"/>.
    /// </summary>
    /// <param name="complaint">The <see cref="Complaint"/> this Action belongs to.</param>
    /// <param name="actionType">The <see cref="ActionType"/> of this Action.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The Complaint ActionItem that was created.</returns>
    ComplaintAction CreateAction(Complaint complaint, ActionType actionType, ApplicationUser? user);

    // Transitions

    void Accept(Complaint complaint, ApplicationUser? user);

    void Assign(Complaint complaint, Office office, ApplicationUser? owner, string? comment, ApplicationUser? user);

    void Close(Complaint complaint, string? comment, ApplicationUser? user);

    /// <summary>
    /// Creates a new <see cref="ComplaintTransition"/>.
    /// </summary>
    /// <param name="complaint">The complaint the Transition is associated with.</param>
    /// <param name="type">The <see cref="TransitionType"/> of Transition to create.</param>
    /// <param name="user">The <see cref="ApplicationUser"/> committing the Transition.</param>
    /// <param name="comment">A comment for the Transition.</param>
    /// <returns>The Complaint Transition that was created.</returns>
    ComplaintTransition CreateTransition(Complaint complaint, TransitionType type, ApplicationUser? user,
        string? comment);
}
