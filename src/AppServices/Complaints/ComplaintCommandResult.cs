using Cts.Domain.Entities.Complaints;

namespace Cts.AppServices.Complaints;

public abstract class ComplaintCommandResult
{
    /// <summary>
    /// If the <see cref="Complaint"/> is successfully created, contains the ID of the new Complaint. 
    /// </summary>
    /// <value>The Complaint ID if the operation succeeded, otherwise null.</value>
    public int? ComplaintId { get; protected init; }

    /// <summary>
    /// A <see cref="List{T}"/> of <see cref="string"/> containing warnings that occurred during the operation.
    /// </summary>
    /// <value>A <see cref="List{T}"/> of <see cref="string"/> instances.</value>
    public List<string> Warnings { get; } = [];

    /// <summary>
    /// Flag indicating whether warnings were generated.
    /// </summary>
    /// <value>True if warnings were generated, otherwise false.</value>
    public bool HasWarnings { get; private set; }

    /// <summary>
    /// Adds a warning to result.
    /// </summary>
    /// <param name="warning">The warning generated.</param>
    public void AddWarning(string warning)
    {
        HasWarnings = true;
        Warnings.Add(warning);
    }
}
