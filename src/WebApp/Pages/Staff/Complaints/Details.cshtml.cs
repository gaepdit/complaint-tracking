using Cts.AppServices.ActionTypes;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DetailsModel(
    IComplaintService complaints,
    IComplaintActionService actionService,
    IActionTypeService actionTypeService,
    IStaffService staffService,
    IAuthorizationService authorization)
    : PageModel
{
    public ComplaintViewDto Item { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [BindProperty]
    public ComplaintActionCreateDto NewAction { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public SelectList ActionItemTypeSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var complaintView = await complaints.FindAsync(id.Value);
        if (complaintView is null) return NotFound();

        var currentUser = await staffService.GetCurrentUserAsync();
        complaintView.CurrentUserOfficeId = currentUser.Office?.Id ?? Guid.Empty;

        await SetPermissionsAsync(complaintView);
        if (complaintView.IsDeleted && !UserCan[ComplaintOperation.ManageDeletions]) return NotFound();

        Item = complaintView;
        NewAction = new ComplaintActionCreateDto(complaintView.Id) { Investigator = currentUser.Name };
        await PopulateSelectListsAsync();
        return Page();
    }

    /// <summary>
    /// Post is used to add a new Action for this Complaint
    /// </summary>
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null) return RedirectToPage("../Index");
        if (NewAction.ComplaintId != id) return BadRequest();

        var complaintView = await complaints.FindAsync(id.Value);
        if (complaintView is null || complaintView.IsDeleted) return BadRequest();

        complaintView.CurrentUserOfficeId = (await staffService.GetCurrentUserAsync()).Office?.Id ?? Guid.Empty;

        await SetPermissionsAsync(complaintView);
        if (!UserCan[ComplaintOperation.EditActions]) return BadRequest();

        if (!ModelState.IsValid)
        {
            Item = complaintView;
            await PopulateSelectListsAsync();
            return Page();
        }

        HighlightId = await actionService.CreateAsync(NewAction);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "New Action successfully added.");
        return RedirectToPage("Details", pageHandler: null, routeValues: new { id }, fragment: HighlightId.ToString());
    }

    private async Task PopulateSelectListsAsync() =>
        ActionItemTypeSelectList = (await actionTypeService.GetActiveListItemsAsync()).ToSelectList();

    private async Task SetPermissionsAsync(ComplaintViewDto item)
    {
        foreach (var operation in ComplaintOperation.AllOperations)
            UserCan[operation] = (await authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }
}
