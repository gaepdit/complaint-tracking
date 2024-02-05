namespace Cts.Domain.Entities.Attachments;

public class AttachmentManager : IAttachmentManager
{
    public Attachment Create(string fileName, long size) =>
        new(Guid.NewGuid())
        {
            FileName = fileName,
            FileExtension = Path.GetExtension(fileName),
            Size = size,
        };
}
