using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.DivisionManager))]
public class ReopenModel(IComplaintService complaintService, IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public ComplaintClosureDto ComplaintClosure { get; set; } = null!;

    public ComplaintViewDto ComplaintView { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id <= 0) return RedirectToPage("Index");

        var complaintView = await complaintService.FindAsync(Id);
        if (complaintView is null) return NotFound();

        if (!await UserCanReviewAsync(complaintView)) return Forbid();

        ComplaintClosure = new ComplaintClosureDto(Id);
        ComplaintView = complaintView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var complaintView = await complaintService.FindAsync(ComplaintClosure.ComplaintId);
        if (complaintView is null || !await UserCanReviewAsync(complaintView))
            return BadRequest();

        var notificationResult = await complaintService.ReopenAsync(ComplaintClosure, this.GetBaseUrl());
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            "The Complaint has been reopened.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { id = ComplaintClosure.ComplaintId });
    }

    private Task<bool> UserCanReviewAsync(ComplaintViewDto item) =>
        authorization.Succeeded(User, item, ComplaintOperation.Reopen);
}
