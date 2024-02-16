using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto.Query;
using Cts.AppServices.Complaints.Permissions;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Staff.Complaints;

public class ApproveModel(IComplaintService complaintService, IAuthorizationService authorizationService)
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

        await complaintService.CloseAsync(ComplaintClosure);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "The Complaint has been approved/closed.");
        return RedirectToPage("Details", new { id = ComplaintClosure.ComplaintId });
    }

    private async Task<bool> UserCanReviewAsync(ComplaintViewDto item) =>
        (await authorizationService.AuthorizeAsync(User, item, ComplaintOperation.Review)).Succeeded;
}
