namespace Cts.Domain.Entities.Complaints;

public interface IComplaintManager
{
    /// <summary>
    /// Creates a new <see cref="Complaint"/>.
    /// </summary>
    /// <param name="createdById">The ID of the user creating the entity.</param>
    /// <returns>The Complaint that was created.</returns>
    public Task<Complaint> CreateNewComplaintAsync(string? createdById);
}
