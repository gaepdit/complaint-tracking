using Cts.Domain.Entities.Attachments;
using Cts.Domain.Entities.Complaints;

namespace Cts.AppServices.Complaints;

public class ComplaintCreateResult : ComplaintCommandResult
{
    /// <summary>
    /// Returns a <see cref="ComplaintCreateResult"/> indicating a successfully created <see cref="Complaint"/>.
    /// </summary>
    /// <param name="complaintId">The ID of the new complaint.</param>
    /// <returns>A <see cref="ComplaintCreateResult"/> indicating a successful operation.</returns>
    public ComplaintCreateResult(int complaintId) => ComplaintId = complaintId;

    /// <summary>
    /// If the <see cref="Complaint"/> is successfully created, contains the number of <see cref="Attachment"/> files saved.
    /// </summary>
    /// <value>The number of Attachment files saved.</value>
    public int NumberOfAttachments { get; private set; }

    /// <summary>
    /// Sets the number of attachments.
    /// </summary>
    /// <param name="numberOfAttachments">The number of attachments uploaded with the Complaint.</param>
    public void SetNumberOfAttachments(int numberOfAttachments) => NumberOfAttachments = numberOfAttachments;
}
