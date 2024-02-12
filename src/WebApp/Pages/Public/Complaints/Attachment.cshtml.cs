using Cts.AppServices.Attachments;
using Cts.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Public.Complaints;

[AllowAnonymous]
public class AttachmentModel : PageModel
{
    public async Task<IActionResult> OnGetAsync([FromServices] IAttachmentService attachmentService,
        [FromRoute] Guid? id, [FromRoute] string? fileName, [FromQuery] bool thumbnail = false) =>
        await this.GetAttachmentFile(attachmentService, id, fileName, thumbnail);
}
