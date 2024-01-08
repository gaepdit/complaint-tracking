using Cts.Domain.Entities.ActionTypes;
using Cts.Domain.Entities.ComplaintActions;

namespace Cts.Domain.Entities.Complaints;

public interface IComplaintManager
{
    /// <summary>
    /// Creates a new <see cref="Complaint"/>.
    /// </summary>
    /// <param name="createdById">The ID of the user creating the entity.</param>
    /// <returns>The Complaint that was created.</returns>
    public Complaint Create(string? createdById);

    /// <summary>
    /// Creates a new <see cref="ComplaintAction"/>.
    /// </summary>
    /// <param name="complaint">The <see cref="Complaint"/> this Action belongs to.</param>
    /// <param name="actionType">The <see cref="ActionType"/> of this Action.</param>
    /// <param name="createdById">The ID of the user creating the entity.</param>
    /// <returns>The Complaint ActionItem that was created.</returns>
    ComplaintAction AddAction(Complaint complaint, ActionType actionType, string? createdById);
}
