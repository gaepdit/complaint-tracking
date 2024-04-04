using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.Manager))]
public class ApproveModel(IComplaintService complaintService, IAuthorizationService authorization)
    : PageModel
{
    [BindProperty]
    public ComplaintClosureDto ComplaintClosure { get; set; } = default!;

    public ComplaintViewDto ComplaintView { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("Index");

        var complaintView = await complaintService.FindAsync(id.Value);
        if (complaintView is null) return NotFound();

        if (!await UserCanReviewAsync(complaintView)) return Forbid();

        ComplaintClosure = new ComplaintClosureDto(id.Value);
        ComplaintView = complaintView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var complaintView = await complaintService.FindAsync(ComplaintClosure.ComplaintId);
        if (complaintView is null || !await UserCanReviewAsync(complaintView))
            return BadRequest();

        var notificationResult = await complaintService.CloseAsync(ComplaintClosure, this.GetBaseUrl());
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            "The Complaint has been approved/closed.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { id = ComplaintClosure.ComplaintId });
    }

    private Task<bool> UserCanReviewAsync(ComplaintViewDto item) =>
        authorization.Succeeded(User, item, ComplaintOperation.Review);
}
