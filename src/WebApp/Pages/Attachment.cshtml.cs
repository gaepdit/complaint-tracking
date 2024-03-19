using Cts.AppServices.Attachments;
using Cts.WebApp.Platform.PageModelHelpers;

namespace Cts.WebApp.Pages;

[AllowAnonymous]
public class AttachmentModel : PageModel
{
    public async Task<IActionResult> OnGetAsync([FromServices] IAttachmentService attachmentService,
        [FromRoute] Guid? id, [FromRoute] string? fileName, [FromQuery] bool thumbnail = false) =>
        await this.GetAttachmentFile(attachmentService, id, fileName, thumbnail);
}
