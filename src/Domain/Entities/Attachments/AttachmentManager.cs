using Cts.Domain.Entities.Complaints;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Http;

namespace Cts.Domain.Entities.Attachments;

public class AttachmentManager : IAttachmentManager
{
    public Attachment Create(IFormFile formFile, Complaint complaint, ApplicationUser? user) =>
        new(Guid.NewGuid())
        {
            Complaint = complaint,
            FileName = Path.GetFileName(formFile.FileName).Trim(),
            FileExtension = Path.GetExtension(formFile.FileName),
            Size = formFile.Length,
            UploadedBy = user,
        };
}
