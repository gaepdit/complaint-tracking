using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.Staff;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using GaEpd.AppLibrary.ListItems;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.Manager))]
public class ReturnModel(
    IComplaintService complaintService,
    IAuthorizationService authorization,
    IOfficeService officeService,
    IStaffService staffService
) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public ComplaintAssignmentDto ComplaintAssignment { get; set; } = null!;

    public ComplaintViewDto ComplaintView { get; private set; } = null!;
    public SelectList OfficesSelectList { get; private set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id <= 0) return RedirectToPage("Index");

        var complaintView = await complaintService.FindAsync(Id);
        if (complaintView is null) return NotFound();

        if (!await UserCanReviewAsync(complaintView)) return Forbid();

        var userOfficeId = (await staffService.GetCurrentUserAsync()).Office?.Id;
        ComplaintAssignment = new ComplaintAssignmentDto(Id)
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
        if (complaintView is null || !await UserCanReviewAsync(complaintView))
            return BadRequest();

        if (!ModelState.IsValid)
        {
            // Invalid ModelState only arises if an Office is not selected.
            await PopulateSelectListsAsync(null);
            ComplaintView = complaintView;
            return Page();
        }

        var notificationResult = await complaintService.ReturnAsync(ComplaintAssignment, this.GetBaseUrl());
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            "The Complaint has been returned.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { id = ComplaintAssignment.ComplaintId });
    }

    private Task<bool> UserCanReviewAsync(ComplaintViewDto complaintView) =>
        authorization.Succeeded(User, complaintView, ComplaintOperation.Review);

    private async Task PopulateSelectListsAsync(Guid? currentOfficeId)
    {
        OfficesSelectList = (await officeService.GetAsListItemsAsync()).ToSelectList();
        StaffSelectList = (await officeService.GetStaffAsListItemsAsync(currentOfficeId)).ToSelectList();
    }
}
