using Cts.AppServices.Attachments;
using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Cts.WebApp.Platform.Settings;

namespace Cts.WebApp.Pages.Staff.Complaints.Attachment;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class AttachmentDeleteModel(IAttachmentService attachmentService, IAuthorizationService authorization)
    : PageModel
{
    [BindProperty]
    public Guid AttachmentId { get; set; }

    public AttachmentViewDto AttachmentView { get; private set; } = null!;
    public int ComplaintId { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? attachmentId)
    {
        if (attachmentId is null) return RedirectToPage("Index");

        var attachmentView = await attachmentService.FindAttachmentAsync(attachmentId.Value);
        if (attachmentView is null) return NotFound();

        var complaintView = await attachmentService.FindComplaintForAttachmentAsync(attachmentId.Value);
        if (complaintView is null) return NotFound();

        if (!await UserCanDeleteAttachmentAsync(complaintView)) return Forbid();

        AttachmentView = attachmentView;
        AttachmentId = attachmentId.Value;
        ComplaintId = complaintView.Id;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var originalAttachment = await attachmentService.FindAttachmentAsync(AttachmentId, token: token);
        if (originalAttachment is null) return BadRequest();

        var complaintView = await attachmentService.FindComplaintForAttachmentAsync(AttachmentId, token: token);
        if (complaintView is null || !await UserCanDeleteAttachmentAsync(complaintView)) return BadRequest();

        await attachmentService.DeleteAttachmentAsync(originalAttachment, AppSettings.AttachmentServiceConfig, token: token);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Attachment successfully deleted.");
        return RedirectToPage("../Details", pageHandler: null, routeValues: new { complaintView.Id },
            fragment: "attachments");
    }

    private Task<bool> UserCanDeleteAttachmentAsync(ComplaintViewDto item) =>
        authorization.Succeeded(User, item, ComplaintOperation.EditAttachments);
}
