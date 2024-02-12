using Cts.Domain.Entities.Complaints;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Http;

namespace Cts.Domain.Entities.Attachments;

public interface IAttachmentManager
{
    /// <summary>
    /// Creates a new <see cref="Attachment"/>.
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="complaint">The <see cref="Complaint"/> the file is attached to.</param>
    /// <param name="user">The user adding the Attachment.</param>
    /// <returns>The Attachment that was created.</returns>
    public Attachment Create(IFormFile formFile, Complaint complaint, ApplicationUser? user);
}
