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

    /// <summary>
    /// Updates the properties of a <see cref="Complaint"/> to indicate that it was accepted by the
    /// assigned <see cref="ApplicationUser"/>. 
    /// </summary>
    /// <param name="complaint">The Complaint that was accepted.</param>
    /// <param name="user">The user committing the change.</param>
    void Accept(Complaint complaint, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="Complaint"/> to indicate that it was assigned or reassigned to
    /// another <see cref="Office"/> or <see cref="ApplicationUser"/>. 
    /// </summary>
    /// <param name="complaint">The Complaint that was assigned.</param>
    /// <param name="office">The Office the Complaint was assigned to.</param>
    /// <param name="owner">The User the Complaint was assigned to.</param>
    /// <param name="user">The user committing the change.</param>
    void Assign(Complaint complaint, Office office, ApplicationUser? owner, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="Complaint"/> to indicate that it was reviewed and approved/closed.
    /// </summary>
    /// <param name="complaint">The Complaint that was closed.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Close(Complaint complaint, string? comment, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a closed <see cref="Complaint"/> to indicate that it was reopened.
    /// </summary>
    /// <param name="complaint">The Complaint that was reopened.</param>
    /// <param name="user">The user committing the change.</param>
    void Reopen(Complaint complaint, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a closed <see cref="Complaint"/> to indicate that a review was requested.
    /// </summary>
    /// <param name="complaint">The Complaint for which a review was requested.</param>
    /// <param name="reviewer">The <see cref="ApplicationUser"/> from whom a review was requested.</param>
    /// <param name="user">The user committing the change.</param>
    void RequestReview(Complaint complaint, ApplicationUser reviewer, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="Complaint"/> to indicate that it was reviewed and returned to
    /// the <see cref="ApplicationUser"/> requesting the review.
    /// </summary>
    /// <param name="complaint">The Complaint for which a review was requested.</param>
    /// <param name="office">The Office the Complaint was returned to.</param>
    /// <param name="owner">The User the Complaint was returned to.</param>
    /// <param name="user">The user committing the change.</param>
    void Return(Complaint complaint, Office office, ApplicationUser? owner, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="Complaint"/> to indicate that it was deleted.
    /// </summary>
    /// <param name="complaint">The Complaint which was deleted.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Delete(Complaint complaint, string? comment, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a deleted <see cref="Complaint"/> to indicate that it was restored.
    /// </summary>
    /// <param name="complaint">The Complaint which was restored.</param>
    /// <param name="user">The user committing the change.</param>
    void Restore(Complaint complaint, ApplicationUser? user);

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
