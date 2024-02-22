﻿using Cts.AppServices.Attachments;
using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Cts.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Staff.Complaints.Attachment;

public class DeleteAttachmentModel(IAttachmentService attachmentService, IAuthorizationService authorizationService)
    : PageModel
{
    [BindProperty]
    public Guid AttachmentId { get; set; }

    public AttachmentViewDto AttachmentView { get; private set; } = default!;
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

        var originalAttachment = await attachmentService.FindAttachmentAsync(AttachmentId, token);
        if (originalAttachment is null) return BadRequest();

        var complaintView = await attachmentService.FindComplaintForAttachmentAsync(AttachmentId, token);
        if (complaintView is null || !await UserCanDeleteAttachmentAsync(complaintView)) return BadRequest();

        await attachmentService.DeleteAttachmentAsync(originalAttachment, AppSettings.AttachmentServiceConfig, token);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Attachment successfully deleted.");
        return RedirectToPage("../Details", pageHandler: null, routeValues: new { complaintView.Id },
            fragment: "attachments");
    }

    private async Task<bool> UserCanDeleteAttachmentAsync(ComplaintViewDto item) =>
        (await authorizationService.AuthorizeAsync(User, item, ComplaintOperation.EditAttachments)).Succeeded;
}
