﻿using Cts.AppServices.ComplaintActions;
using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Staff.Complaints;

public class RestoreActionModel(
    IComplaintActionService actionService,
    IComplaintService complaintService,
    IAuthorizationService authorizationService)
    : PageModel
{
    [BindProperty]
    public Guid ActionItemId { get; set; }

    [TempData]
    public Guid HighlightId { get; set; }

    public ComplaintActionViewDto ActionItemView { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("Index");

        var actionItem = await actionService.FindAsync(actionId.Value);
        if (actionItem is null) return NotFound();

        var complaintView = await complaintService.FindAsync(actionItem.ComplaintId);
        if (complaintView is null) return NotFound();

        if (!await UserCanRestoreActionItemsAsync(complaintView)) return Forbid();

        if (!actionItem.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "Complaint Action cannot be restored because it is not deleted.");
            return RedirectToPage("Details", routeValues: new { complaintView.Id });
        }

        ActionItemView = actionItem;
        ActionItemId = actionId.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var originalActionItem = await actionService.FindAsync(ActionItemId);
        if (originalActionItem is null || !originalActionItem.IsDeleted) return BadRequest();

        var complaintView = await complaintService.FindAsync(originalActionItem.ComplaintId);
        if (complaintView is null || !await UserCanRestoreActionItemsAsync(complaintView))
            return BadRequest();

        await actionService.RestoreAsync(ActionItemId);
        HighlightId = ActionItemId;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint Action successfully restored.");
        return RedirectToPage("Details", pageHandler: null, routeValues: new { complaintView.Id },
            fragment: HighlightId.ToString());
    }

    private async Task<bool> UserCanRestoreActionItemsAsync(ComplaintViewDto item) =>
        (await authorizationService.AuthorizeAsync(User, item, ComplaintOperation.EditActions)).Succeeded;
}
