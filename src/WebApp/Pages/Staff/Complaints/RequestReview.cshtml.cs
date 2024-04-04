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
    [BindProperty]
    public ComplaintRequestReviewDto ComplaintRequestReview { get; set; } = default!;

    public ComplaintViewDto ComplaintView { get; private set; } = default!;
    public SelectList ReviewersSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("Index");

        var complaintView = await complaintService.FindAsync(id.Value);
        if (complaintView is null) return NotFound();

        if (!await UserCanRequestReviewAsync(complaintView)) return Forbid();

        if (complaintView.CurrentOffice is null)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "The Complaint must be assigned to an Office in order to request a review.");
            return RedirectToPage("Details", new { id });
        }

        await PopulateSelectListsAsync(complaintView.CurrentOffice.Id);

        if (!ReviewersSelectList.Any())
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "The current assigned Office does not have any managers to review/approve Complaints. Please contact a Division Manager for assistance.");
            return RedirectToPage("Details", new { id });
        }

        ComplaintRequestReview = new ComplaintRequestReviewDto(id.Value);
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

        var emailResult = await complaintService.RequestReviewAsync(ComplaintRequestReview, this.GetBaseUrl());
        TempData.SetDisplayMessage(
            emailResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            "The Complaint has been submitted for review.", emailResult.FailureMessage);

        return RedirectToPage("Details", new { id = ComplaintRequestReview.ComplaintId });
    }

    private Task<bool> UserCanRequestReviewAsync(ComplaintViewDto complaintView) =>
        authorization.Succeeded(User, complaintView, ComplaintOperation.RequestReview);

    private async Task PopulateSelectListsAsync(Guid currentOfficeId) =>
        ReviewersSelectList = (await staffService.GetUsersInRoleAsListItemsAsync(AppRole.ManagerRole, currentOfficeId))
            .ToSelectList();
}
