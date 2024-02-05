using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Public.Complaints;

[AllowAnonymous]
public class AttachmentModel(IComplaintService complaintService, IAttachmentFileService attachmentFileService)
    : PageModel
{
    public async Task<IActionResult> OnGetAsync([FromRoute] Guid? id, [FromRoute] string? fileName,
        [FromQuery] bool thumbnail = false)
    {
        if (id is null) return NotFound();
        var attachmentView = await complaintService.FindPublicAttachmentAsync(id.Value);
        return await new AttachmentFileHandler(this, attachmentFileService)
            .GetAttachmentFile(id.Value, attachmentView, fileName, thumbnail);
    }
}
