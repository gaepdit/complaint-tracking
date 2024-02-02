namespace Cts.Domain.Entities.Attachments;

public interface IAttachmentManager
{
    /// <summary>
    /// Creates a new <see cref="Attachment"/>.
    /// </summary>
    /// <param name="fileName">The name of the uploaded file.</param>
    /// <param name="size">The size of the uploaded file.</param>
    /// <returns>The Attachment that was created.</returns>
    public Attachment Create(string fileName, long size);
}
