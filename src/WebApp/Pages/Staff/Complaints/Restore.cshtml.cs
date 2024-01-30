using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Staff.Complaints;

public class RestoreModel(IComplaintService complaintService, IAuthorizationService authorizationService)
    : PageModel
{
    [BindProperty]
    public int ComplaintId { get; set; }

    public ComplaintViewDto ComplaintView { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("Index");

        var complaintView = await complaintService.FindAsync(id.Value);
        if (complaintView is null) return NotFound();

        if (!await UserCanManageDeletionsAsync(complaintView)) return Forbid();

        if (!complaintView.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "Complaint cannot be restored because it is not deleted.");
            return RedirectToPage("Details", routeValues: new { id = ComplaintId });
        }

        ComplaintView = complaintView;
        ComplaintId = id.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var complaintView = await complaintService.FindAsync(ComplaintId);
        if (complaintView is null || !complaintView.IsDeleted || !await UserCanManageDeletionsAsync(complaintView))
            return BadRequest();

        await complaintService.RestoreAsync(ComplaintId);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint successfully restored.");
        return RedirectToPage("Details", new { id = ComplaintId });
    }

    private async Task<bool> UserCanManageDeletionsAsync(ComplaintViewDto item) =>
        (await authorizationService.AuthorizeAsync(User, item, ComplaintOperation.ManageDeletions)).Succeeded;
}
