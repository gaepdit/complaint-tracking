using Cts.AppServices.ActionTypes;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using GaEpd.AppLibrary.ListItems;

namespace Cts.WebApp.Pages.Staff.ComplaintActions;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class EditActionModel(
    IActionService actionService,
    IComplaintService complaintService,
    IActionTypeService actionTypeService,
    IAuthorizationService authorization)
    : PageModel
{
    [BindProperty]
    public Guid ActionItemId { get; set; }

    [BindProperty]
    public ActionUpdateDto ActionItemUpdate { get; set; } = null!;

    [TempData]
    public Guid HighlightId { get; set; }

    public ComplaintViewDto ComplaintView { get; private set; } = null!;
    public SelectList ActionItemTypeSelectList { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("Index");

        var actionItem = await actionService.FindForUpdateAsync(actionId.Value);
        if (actionItem is null) return NotFound();

        var complaintView = await complaintService.FindAsync(actionItem.ComplaintId);
        if (complaintView is null) return NotFound();

        if (!await UserCanEditActionItemsAsync(complaintView)) return Forbid();

        ActionItemUpdate = actionItem;
        ActionItemId = actionId.Value;
        ComplaintView = complaintView;
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var originalActionItem = await actionService.FindAsync(ActionItemId);
        if (originalActionItem is null || originalActionItem.IsDeleted) return BadRequest();

        var complaintView = await complaintService.FindAsync(originalActionItem.ComplaintId);
        if (complaintView is null || !await UserCanEditActionItemsAsync(complaintView))
            return BadRequest();

        if (!ModelState.IsValid)
        {
            ComplaintView = complaintView;
            await PopulateSelectListsAsync();
            return Page();
        }

        await actionService.UpdateAsync(ActionItemId, ActionItemUpdate);

        HighlightId = ActionItemId;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint Action successfully updated.");
        return RedirectToPage("../Complaints/Details", pageHandler: null, routeValues: new { complaintView.Id },
            fragment: HighlightId.ToString());
    }

    private async Task PopulateSelectListsAsync() =>
        ActionItemTypeSelectList = (await actionTypeService.GetAsListItemsAsync()).ToSelectList();

    private Task<bool> UserCanEditActionItemsAsync(ComplaintViewDto item) =>
        authorization.Succeeded(User, item, ComplaintOperation.EditActions);
}
