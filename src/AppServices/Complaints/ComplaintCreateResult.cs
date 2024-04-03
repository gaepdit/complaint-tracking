using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.Complaints;

namespace Cts.AppServices.Complaints;

public class ComplaintCreateResult
{
    /// <summary>
    /// If the <see cref="Complaint"/> is successfully created, contains the ID of the new Complaint. 
    /// </summary>
    /// <value>The Complaint ID if the operation succeeded, otherwise null.</value>
    public int? ComplaintId { get; private init; }

    /// <summary>
    /// If the <see cref="Complaint"/> is successfully created, contains the number of <see cref="Attachment"/> files saved.
    /// </summary>
    /// <value>The number of Attachment files saved.</value>
    public int NumberOfAttachments { get; private set; }

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
    /// Returns an <see cref="ComplaintCreateResult"/> indicating a successfully created <see cref="Complaint"/>.
    /// </summary>
    /// <param name="complaintId">The ID of the new complaint.</param>
    /// <returns>An <see cref="ComplaintCreateResult"/> indicating a successful operation.</returns>
    public static ComplaintCreateResult Create(int complaintId) => new() { ComplaintId = complaintId };

    /// <summary>
    /// Sets the number of attachments.
    /// </summary>
    /// <param name="numberOfAttachments">The number of attachments uploaded with the Complaint.</param>
    public void SetNumberOfAttachments(int numberOfAttachments) => NumberOfAttachments = numberOfAttachments;

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
