using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using GaEpd.AppLibrary.ListItems;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class RequestReviewModel(
    IComplaintService complaintService,
    IAuthorizationService authorization,
    IStaffService staffService
) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public ComplaintRequestReviewDto ComplaintRequestReview { get; set; } = null!;

    public ComplaintViewDto ComplaintView { get; private set; } = null!;
    public SelectList ReviewersSelectList { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id <= 0) return RedirectToPage("Index");

        var complaintView = await complaintService.FindAsync(Id);
        if (complaintView is null) return NotFound();

        if (!await UserCanRequestReviewAsync(complaintView)) return Forbid();

        if (complaintView.CurrentOffice is null)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "The Complaint must be assigned to an Office in order to request a review.");
            return RedirectToPage("Details", new { Id });
        }

        await PopulateSelectListsAsync(complaintView.CurrentOffice.Id);

        if (!ReviewersSelectList.Any())
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "The current assigned Office does not have any managers to review/approve Complaints. Please contact a Division Manager for assistance.");
            return RedirectToPage("Details", new { Id });
        }

        ComplaintRequestReview = new ComplaintRequestReviewDto(Id);
        ComplaintView = complaintView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var complaintView = await complaintService.FindAsync(ComplaintRequestReview.ComplaintId);
        if (complaintView?.CurrentOffice is null || !await UserCanRequestReviewAsync(complaintView))
            return BadRequest();

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(complaintView.CurrentOffice.Id);
            if (!ReviewersSelectList.Any()) return BadRequest();
            ComplaintView = complaintView;
            return Page();
        }

        var notificationResult = await complaintService.RequestReviewAsync(ComplaintRequestReview, this.GetBaseUrl());
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            "The Complaint has been submitted for review.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { id = ComplaintRequestReview.ComplaintId });
    }

    private Task<bool> UserCanRequestReviewAsync(ComplaintViewDto complaintView) =>
        authorization.Succeeded(User, complaintView, ComplaintOperation.RequestReview);

    private async Task PopulateSelectListsAsync(Guid currentOfficeId) =>
        ReviewersSelectList = (await staffService.GetUsersInRoleAsListItemsAsync(AppRole.ManagerRole, currentOfficeId))
            .ToSelectList();
}
