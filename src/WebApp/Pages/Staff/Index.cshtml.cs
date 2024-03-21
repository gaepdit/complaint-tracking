using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using System.ComponentModel.DataAnnotations;

namespace Cts.WebApp.Pages.Staff;

public class DashboardIndexModel(
    IComplaintService complaints,
    IStaffService staffService,
    IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Please enter a complaint ID.")]
    [Display(Name = "Complaint ID")]
    public string? FindId { get; set; }


    // Staff results
    public bool IsStaff { get; private set; }
    public IReadOnlyList<ComplaintSearchResultDto> MyNewComplaints { get; private set; } = [];
    public IReadOnlyList<ComplaintSearchResultDto> MyOpenComplaints { get; private set; } = [];

    // Manager results
    public bool IsManager { get; private set; }
    public IReadOnlyList<ComplaintSearchResultDto> MgrComplaintsPendingReview { get; private set; } = [];
    public IReadOnlyList<ComplaintSearchResultDto> MgrUnassignedComplaints { get; private set; } = [];
    public IReadOnlyList<ComplaintSearchResultDto> MgrUnacceptedComplaints { get; private set; } = [];

    // Assignor results
    public IReadOnlyList<ComplaintSearchResultDto> AssignorUnassignedComplaints { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken token)
    {
        var userId = (await staffService.GetCurrentUserAsync()).Id;

        IsStaff = (await authorization.AuthorizeAsync(User, Policies.StaffUser)).Succeeded;
        if (IsStaff)
        {
            MyNewComplaints = await complaints.GetNewComplaintsForUserAsync(userId, token);
            MyOpenComplaints = await complaints.GetOpenComplaintsForUserAsync(userId, token);
        }

        IsManager = (await authorization.AuthorizeAsync(User, Policies.Manager)).Succeeded;
        if (IsManager)
        {
            
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        if (!int.TryParse(FindId, out var idInt))
        {
            ModelState.AddModelError(nameof(FindId), "Complaint ID must be a number.");
        }
        else if (!await complaints.ExistsAsync(idInt))
        {
            ModelState.AddModelError(nameof(FindId), "The Complaint ID entered does not exist.");
        }

        if (!ModelState.IsValid) return Page();
        return RedirectToPage("Complaints/Details", new { id = FindId });
    }
}
