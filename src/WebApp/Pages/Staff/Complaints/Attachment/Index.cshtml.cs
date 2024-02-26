using Cts.AppServices.Attachments;
using Cts.AppServices.Permissions;
using Cts.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Staff.Complaints.Attachment;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class AttachmentModel : PageModel
{
    public async Task<IActionResult> OnGetAsync([FromServices] IAttachmentService attachmentService,
        [FromRoute] Guid? id, [FromRoute] string? fileName, [FromQuery] bool thumbnail = false) =>
        await this.GetAttachmentFile(attachmentService, id, fileName, thumbnail);
}
