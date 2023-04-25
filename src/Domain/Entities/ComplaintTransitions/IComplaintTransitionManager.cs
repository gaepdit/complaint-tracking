using Cts.Domain.Entities.Complaints;
using Cts.Domain.Identity;

namespace Cts.Domain.Entities.ComplaintTransitions;

/// <summary>
/// A manager for managing Concerns.
/// </summary>
public interface IComplaintTransitionManager
{
    /// <summary>
    /// Creates a new <see cref="ComplaintTransition"/>.
    /// </summary>
    /// <param name="complaint">The complaint the Transition is associated with.</param>
    /// <param name="type">The <see cref="TransitionType"/> of Transition to create.</param>
    /// <param name="user">The <see cref="ApplicationUser"/> commiting the Transition.</param>
    /// <returns>The Complaint Transition that was created.</returns>
    ComplaintTransition Create(Complaint complaint, TransitionType type, ApplicationUser? user);
}
