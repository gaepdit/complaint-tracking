﻿using Cts.AppServices.AuthorizationPolicies;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;

namespace Cts.WebApp.Pages.Staff.ComplaintActions;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class RestoreActionModel(
    IActionService actionService,
    IComplaintService complaintService,
    IAuthorizationService authorization)
    : PageModel
{
    [BindProperty]
    public Guid ActionItemId { get; set; }

    [TempData]
    public Guid HighlightId { get; set; }

    public ActionViewDto ActionItemView { get; private set; } = null!;

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
            return RedirectToPage("../Complaints/Details", routeValues: new { complaintView.Id });
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
        return RedirectToPage("../Complaints/Details", pageHandler: null, routeValues: new { complaintView.Id },
            fragment: HighlightId.ToString());
    }

    private Task<bool> UserCanRestoreActionItemsAsync(ComplaintViewDto item) =>
        authorization.Succeeded(User, item, ComplaintOperation.EditActions);
}
