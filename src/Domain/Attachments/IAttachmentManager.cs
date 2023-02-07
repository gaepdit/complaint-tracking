using Cts.Domain.Exceptions;

namespace Cts.Domain.Attachments;

/// <summary>
/// A manager for managing Attachments.
/// </summary>
public interface IAttachmentManager
{
    /// <summary>
    /// Creates a new <see cref="Attachment"/>.
    /// </summary>
    /// <param name="complaintId">The ID of the complaint to which the attachment is attached.</param>
    /// <param name="fileName">The name (including extension) of the attachment file.</param>
    /// <param name="fileSize">The file length in bytes.</param>
    /// <exception cref="InvalidFileTypeException">Thrown if the file type is invalid.</exception>
    /// <returns>The Attachment that was created.</returns>
    Attachment Create(int complaintId, string fileName, long fileSize);
}
