using Cts.AppServices.Attachments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Platform.PageModelHelpers;

internal class AttachmentFileHandler(PageModel page, IAttachmentFileService attachmentFileService)
{
    public async Task<IActionResult> GetAttachmentFile(Guid id, AttachmentViewDto? attachmentView, string? fileName,
        bool thumbnail)
    {
        if (attachmentView is null || string.IsNullOrWhiteSpace(attachmentView.FileName))
            return page.NotFound($"Attachment ID not found: {id}");

        // Fix bad URL (fileName route parameter is not considered definitive).
        if (fileName != attachmentView.FileName)
            return thumbnail
                ? page.RedirectToPage("Attachment", new { id, attachmentView.FileName, thumbnail = true })
                : page.RedirectToPage("Attachment", new { id, attachmentView.FileName });

        var fileBytes = await attachmentFileService.GetAttachmentFileAsync(attachmentView.FileId, thumbnail);

        if (fileBytes.Length > 0) return page.File(fileBytes, FileTypes.GetContentType(attachmentView.FileExtension));

        return thumbnail
            ? page.LocalRedirect("~/images/Georgia_404.svg")
            : page.NotFound($"File not found: {attachmentView.FileName}.");
    }
}
