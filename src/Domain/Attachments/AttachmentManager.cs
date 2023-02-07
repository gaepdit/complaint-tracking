namespace Cts.Domain.Attachments;

public class AttachmentManager : IAttachmentManager
{
    public Attachment Create(int complaintId, string fileName, long fileSize)
    {
        // TODO: Check for invalid file type
        
        var attachment = new Attachment(Guid.NewGuid())
        {
            ComplaintId = complaintId,
            FileName = fileName,
            FileExtension = Path.GetExtension(fileName),
            Size = fileSize,
            DateUploaded = DateTime.Now,
        };

        return attachment;
    }
}
