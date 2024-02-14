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
    public int NumberOfAttachments { get; private init; }

    /// <summary>
    /// A <see cref="List{T}"/> of <see cref="string"/> containing warnings that occurred during the operation.
    /// </summary>
    /// <value>A <see cref="List{T}"/> of <see cref="string"/> instances.</value>
    private List<string> Warnings { get; } = [];

    public string WarningsDisplay => Warnings.Count == 0 ? string.Empty : "* " + string.Join(" * ", Warnings);

    /// <summary>
    /// Flag indicating whether warnings were generated.
    /// </summary>
    /// <value>True if warnings were generated, otherwise false.</value>
    public bool HasWarnings { get; private init; }

    /// <summary>
    /// Returns an <see cref="ComplaintCreateResult"/> indicating a successfully created <see cref="Complaint"/>.
    /// </summary>
    /// <param name="complaintId">The ID of the new complaint.</param>
    /// <returns>An <see cref="ComplaintCreateResult"/> indicating a successful operation.</returns>
    public static ComplaintCreateResult Success(int complaintId) => new() { ComplaintId = complaintId };

    /// <summary>
    /// Returns an <see cref="ComplaintCreateResult"/> indicating a successfully created <see cref="Complaint"/>.
    /// </summary>
    /// <param name="complaintId">The ID of the new complaint.</param>
    /// <param name="numberOfAttachments">The number of attachments uploaded with the Complaint.</param>
    /// <returns>An <see cref="ComplaintCreateResult"/> indicating a successful operation.</returns>
    public static ComplaintCreateResult Success(int complaintId, int numberOfAttachments) =>
        new() { ComplaintId = complaintId, NumberOfAttachments = numberOfAttachments };

    /// <summary>
    /// Returns an <see cref="ComplaintCreateResult"/> indicating a successfully created <see cref="Complaint"/>,
    /// with a list of <paramref name="warnings"/>.
    /// </summary>
    /// <param name="complaintId">The ID of the new complaint.</param>
    /// <param name="warnings">The warnings generated.</param>
    /// <returns>An <see cref="ComplaintCreateResult"/> indicating a successful operation, with a list
    /// of <paramref name="warnings"/>.</returns>
    public static ComplaintCreateResult Success(int complaintId, IEnumerable<string> warnings)
    {
        var result = new ComplaintCreateResult { ComplaintId = complaintId, HasWarnings = true };
        result.Warnings.AddRange(warnings);
        return result;
    }
}
