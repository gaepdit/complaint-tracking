using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Files;
using Cts.Domain.Entities.Attachments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Complaints;

[Authorize]
public class AttachmentModel : PageModel
{
    public async Task<IActionResult> OnGetAsync(
        [FromServices] IComplaintService complaintService,
        [FromServices] IFileService fileService,
        [FromRoute] Guid? id,
        [FromRoute] string? fileName,
        [FromQuery] bool thumbnail = false)
    {
        if (id is null) return NotFound();

        var item = await complaintService.GetAttachmentAsync(id.Value);
        if (item is null || string.IsNullOrWhiteSpace(item.FileName))
            return NotFound($"Attachment ID not found: {id.Value}");

        if (fileName != item.FileName)
            return thumbnail
                ? RedirectToPage("Attachment", new { id, item.FileName, thumbnail = true })
                : RedirectToPage("Attachment", new { id, item.FileName });

        var fileBytes = thumbnail
            ? await fileService.GetFileAsync(item.AttachmentFileName, Attachment.DefaultThumbnailsLocation)
            : await fileService.GetFileAsync(item.AttachmentFileName, Attachment.DefaultAttachmentsLocation);

        return fileBytes.Length > 0
            ? File(fileBytes, FileTypes.GetContentType(item.FileExtension))
            : FileNotFound(item);
    }

    private IActionResult FileNotFound(AttachmentViewDto item) =>
        item.IsImage || FileTypes.FilenameImpliesImage(item.FileExtension)
            ? Redirect("~/images/Georgia_404.svg")
            : NotFound($"File not available: {item.FileName}");
}
