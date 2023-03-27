using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Complaints;

[Authorize]
public class DetailsModel : PageModel
{
    // Properties
    public ComplaintViewDto Item { get; private set; } = default!;

    // Control properties
    public bool UserIsOwner { get; set; }
    public bool CanDelete { get; set; }
    public bool CanAccept { get; set; }
    public bool CanEditDetails { get; set; }
    public bool CanEditAll { get; set; }
    public bool CanReview { get; set; }
    public bool CanAssign { get; set; }
    public bool CanRequestReview { get; set; }
    public bool CanReassign { get; set; }
    public bool CanReopen { get; set; }
    public bool CanEditAttachments { get; set; }

    // Services
    private readonly IComplaintAppService _complaints;
    private readonly IStaffAppService _staff;

    public DetailsModel(IComplaintAppService complaints, IStaffAppService staff)
    {
        _complaints = complaints;
        _staff = staff;
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var staff = await _staff.GetCurrentUserAsync();
        if (staff is not { Active: true }) return Forbid();

        if (id is null) return RedirectToPage("../Index");
        var item = await _complaints.GetAsync(id.Value);
        if (item is null) return NotFound();

        Item = item;

        // TODO: Set control properties

        return Page();
    }
}
