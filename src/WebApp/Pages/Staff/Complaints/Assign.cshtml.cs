using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Staff.Complaints;

public class AssignModel(
    IComplaintService complaintService,
    IAuthorizationService authorizationService,
    IOfficeService officeService,
    IStaffService staffService
) : PageModel
{
    [BindProperty]
    public ComplaintAssignmentDto ComplaintAssignment { get; set; } = default!;

    public ComplaintViewDto ComplaintView { get; private set; } = default!;
    public SelectList OfficesSelectList { get; private set; } = default!;
    public SelectList StaffSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("Index");

        var complaintView = await complaintService.FindAsync(id.Value);
        if (complaintView is null) return NotFound();

        if (!await UserCanAssignAsync(complaintView)) return Forbid();

        var userOfficeId = (await staffService.GetCurrentUserAsync()).Office?.Id;
        ComplaintAssignment = new ComplaintAssignmentDto(id.Value)
        {
            OfficeId = complaintView.CurrentOffice?.Id ?? userOfficeId,
            OwnerId = complaintView.CurrentOwner?.Id,
        };
        ComplaintView = complaintView;
        await PopulateSelectListsAsync(ComplaintAssignment.OfficeId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var complaintView = await complaintService.FindAsync(ComplaintAssignment.ComplaintId);
        if (complaintView is null || !await UserCanAssignAsync(complaintView))
            return BadRequest();

        if (!ModelState.IsValid)
        {
            // Invalid ModelState only arises if an Office is not selected.
            await PopulateSelectListsAsync(null);
            ComplaintView = complaintView;
            return Page();
        }

        if (await complaintService.AssignAsync(ComplaintAssignment, complaintView))
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "The Complaint has been assigned.");
        }
        else
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "The Complaint assignment has not changed.");
        }

        return RedirectToPage("Details", new { id = ComplaintAssignment.ComplaintId });
    }

    private async Task<bool> UserCanAssignAsync(ComplaintViewDto complaintView) =>
        complaintView.CurrentOwner is null
            ? (await authorizationService.AuthorizeAsync(User, complaintView, ComplaintOperation.Assign)).Succeeded
            : (await authorizationService.AuthorizeAsync(User, complaintView, ComplaintOperation.Reassign)).Succeeded;

    private async Task PopulateSelectListsAsync(Guid? officeId)
    {
        OfficesSelectList = (await officeService.GetAsListItemsAsync()).ToSelectList();
        StaffSelectList = (await officeService.GetStaffAsListItemsAsync(officeId)).ToSelectList();
    }
}
