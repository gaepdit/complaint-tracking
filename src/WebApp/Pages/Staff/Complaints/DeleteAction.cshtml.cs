using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Staff.Complaints;

public class DeleteActionModel(
    IComplaintActionService actionService,
    IComplaintService complaintService,
    IAuthorizationService authorizationService)
    : PageModel
{
    [BindProperty]
    public Guid ActionItemId { get; set; }

    public ComplaintActionViewDto ActionItemView { get; private set; } = default!;
    
    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("Index");

        var actionItem = await actionService.FindAsync(actionId.Value);
        if (actionItem is null) return NotFound();

        var complaintView = await complaintService.FindAsync(actionItem.ComplaintId);
        if (complaintView is null) return NotFound();

        if (!await UserCanDeleteActionItemsAsync(complaintView)) return Forbid();

        ActionItemView = actionItem;
        ActionItemId = actionId.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var originalActionItem = await actionService.FindAsync(ActionItemId);
        if (originalActionItem is null) return BadRequest();

        var complaintView = await complaintService.FindAsync(originalActionItem.ComplaintId);
        if (complaintView is null || !await UserCanDeleteActionItemsAsync(complaintView))
            return BadRequest();

        await actionService.DeleteAsync(ActionItemId);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint Action successfully deleted.");
        return RedirectToPage("Details", new { complaintView.Id });
    }

    private async Task<bool> UserCanDeleteActionItemsAsync(ComplaintViewDto item) =>
        (await authorizationService.AuthorizeAsync(User, item, ComplaintOperation.EditActions)).Succeeded;
}
