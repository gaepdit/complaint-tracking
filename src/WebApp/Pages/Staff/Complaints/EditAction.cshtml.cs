using Cts.AppServices.ActionTypes;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Staff.Complaints;

public class EditActionModel(
    IComplaintActionService actionService,
    IComplaintService complaintService,
    IActionTypeService actionTypeService,
    IAuthorizationService authorizationService)
    : PageModel
{
    [BindProperty]
    public Guid ActionItemId { get; set; }

    [BindProperty]
    public ComplaintActionUpdateDto ActionItemUpdate { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public ComplaintViewDto ComplaintView { get; private set; } = default!;
    public SelectList ActionItemTypeSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("Index");

        var actionItem = await actionService.FindForUpdateAsync(actionId.Value);
        if (actionItem is null) return NotFound();

        var complaintView = await complaintService.FindAsync(actionItem.ComplaintId);
        if (complaintView is null) return NotFound();

        if (!await UserCanEditActionItemsAsync(complaintView)) return Forbid();

        if (complaintView.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "Complaint Actions cannot be edit because the complaint is deleted.");
            return RedirectToPage("Details", routeValues: new { complaintView.Id });
        }

        ActionItemUpdate = actionItem;
        ActionItemId = actionId.Value;
        ComplaintView = complaintView;
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var originalActionItem = await actionService.FindAsync(ActionItemId);
        if (originalActionItem is null || originalActionItem.IsDeleted) return BadRequest();

        var complaintView = await complaintService.FindAsync(originalActionItem.ComplaintId);
        if (complaintView is null || !await UserCanEditActionItemsAsync(complaintView))
            return BadRequest();

        await actionService.UpdateAsync(ActionItemId, ActionItemUpdate);

        HighlightId = ActionItemId;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint Action successfully updated.");
        return RedirectToPage("Details", pageHandler: null, routeValues: new { complaintView.Id },
            fragment: HighlightId.ToString());
    }

    private async Task PopulateSelectListsAsync() =>
        ActionItemTypeSelectList = (await actionTypeService.GetAsListItemsAsync()).ToSelectList();

    private async Task<bool> UserCanEditActionItemsAsync(ComplaintViewDto item) =>
        (await authorizationService.AuthorizeAsync(User, item, ComplaintOperation.EditActions)).Succeeded;
}
