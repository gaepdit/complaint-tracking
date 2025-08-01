using Cts.AppServices.AuthorizationPolicies;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.DivisionManager))]
public class RestoreModel(IComplaintService complaintService, IAuthorizationService authorization)
    : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public ComplaintClosureDto ComplaintClosure { get; set; } = null!;

    public ComplaintViewDto ComplaintView { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id <= 0) return RedirectToPage("Index");

        var complaintView = await complaintService.FindAsync(Id, includeDeleted: true);
        if (complaintView is null || !complaintView.IsDeleted) return NotFound();

        if (!await UserCanManageDeletionsAsync(complaintView)) return Forbid();

        ComplaintClosure = new ComplaintClosureDto(Id);
        ComplaintView = complaintView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var complaintView = await complaintService.FindAsync(ComplaintClosure.ComplaintId, includeDeleted: true);
        if (complaintView is null || !complaintView.IsDeleted || !await UserCanManageDeletionsAsync(complaintView))
            return BadRequest();

        await complaintService.RestoreAsync(ComplaintClosure);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint successfully restored.");
        return RedirectToPage("Details", new { id = ComplaintClosure.ComplaintId });
    }

    private Task<bool> UserCanManageDeletionsAsync(ComplaintViewDto item) =>
        authorization.Succeeded(User, item, ComplaintOperation.ManageDeletions);
}
