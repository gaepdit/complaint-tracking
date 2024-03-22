using Cts.AppServices.Attachments;
using Cts.WebApp.Platform.Settings;
using PublicComplaint = Cts.WebApp.Pages;
using StaffComplaint = Cts.WebApp.Pages.Staff.Complaints;

namespace Cts.WebApp.Platform.PageModelHelpers;

internal static class AttachmentFileHandler
{
    public static Task<IActionResult> GetAttachmentFile(this PublicComplaint.AttachmentModel page,
        IAttachmentService attachmentService, Guid? id, string? fileName, bool thumbnail) =>
        page.GetAttachmentFile(attachmentService, id, fileName, thumbnail, forPublic: true);

    public static Task<IActionResult> GetAttachmentFile(this StaffComplaint.AttachmentModel page,
        IAttachmentService attachmentService, Guid? id, string? fileName, bool thumbnail) =>
        page.GetAttachmentFile(attachmentService, id, fileName, thumbnail, forPublic: false);

    private static async Task<IActionResult> GetAttachmentFile(this PageModel page,
        IAttachmentService attachmentService, Guid? id, string? fileName, bool thumbnail, bool forPublic)
    {
        if (id is null) return page.NotFound();
        var attachmentView = forPublic
            ? await attachmentService.FindPublicAttachmentAsync(id.Value)
            : await attachmentService.FindAttachmentAsync(id.Value);

        if (attachmentView is null || string.IsNullOrWhiteSpace(attachmentView.FileName))
            return page.NotFound($"Attachment ID not found: {id}");

        // Fix bad URL (fileName route parameter is not considered definitive).
        if (fileName != attachmentView.FileName)
            return thumbnail
                ? page.RedirectToPage("Attachment", new { id, attachmentView.FileName, thumbnail = true })
                : page.RedirectToPage("Attachment", new { id, attachmentView.FileName });

        var fileBytes = await attachmentService.GetAttachmentFileAsync(attachmentView.FileId, thumbnail,
            AppSettings.AttachmentServiceConfig);

        if (fileBytes.Length > 0) return page.File(fileBytes, FileTypes.GetContentType(attachmentView.FileExtension));

        return thumbnail
            ? page.LocalRedirect("~/images/Georgia_404.svg")
            : page.NotFound($"File not found: {attachmentView.FileName}.");
    }
}
