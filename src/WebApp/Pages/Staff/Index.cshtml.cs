using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.Staff;
using System.ComponentModel.DataAnnotations;

namespace Cts.WebApp.Pages.Staff;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DashboardIndexModel(
    IComplaintService complaintService,
    IStaffService staffService,
    IOfficeService officeService,
    IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Please enter a complaint ID.")]
    [Display(Name = "Complaint ID")]
    public string? FindId { get; set; }

    // Staff results
    public bool IsStaff { get; private set; }
    public DashboardCard MyNewComplaints { get; private set; } = null!;
    public DashboardCard MyOpenComplaints { get; private set; } = null!;

    // Manager results
    public bool IsManager { get; private set; }
    public DashboardCard MgrReviewPending { get; private set; } = null!;
    public DashboardCard MgrUnacceptedComplaints { get; private set; } = null!;
    public DashboardCard MgrUnassignedComplaints { get; private set; } = null!;

    // Assignor results
    public bool IsAssignor => AssignorUnassignedComplaints.Count > 0;
    public List<DashboardCard> AssignorUnassignedComplaints { get; private set; } = [];

    public Task<PageResult> OnGetAsync(CancellationToken token) => BuildDashboardAsync(token);

    public async Task<PageResult> BuildDashboardAsync(CancellationToken token)
    {
        var user = await staffService.GetCurrentUserAsync();
        var officeName = user.Office?.Name;
        var officeId = user.Office?.Id ?? Guid.Empty;

        IsStaff = await authorization.Succeeded(User, Policies.StaffUser);
        if (IsStaff)
        {
            MyNewComplaints = new DashboardCard("My new complaints")
                { Complaints = await complaintService.GetNewComplaintsForUserAsync(user.Id, token: token) };
            MyOpenComplaints = new DashboardCard("My open complaints")
                { Complaints = await complaintService.GetOpenComplaintsForUserAsync(user.Id, token: token) };
        }

        IsManager = await authorization.Succeeded(User, Policies.Manager);
        if (IsManager)
        {
            MgrReviewPending = new DashboardCard("My complaints to review")
                { Complaints = await complaintService.GetReviewPendingComplaintsForUserAsync(user.Id, token: token) };
            MgrUnassignedComplaints = new DashboardCard($"Complaints in {officeName} that have not been assigned")
                { Complaints = await complaintService.GetUnassignedComplaintsForOfficeAsync(officeId, token: token) };
            MgrUnacceptedComplaints =
                new DashboardCard($"Complaints in {officeName} that have been assigned but not accepted")
                    { Complaints = await complaintService.GetUnacceptedComplaintsForOfficeAsync(officeId, token: token) };
        }

        var assignorOfficeList = (await officeService.GetOfficesForAssignorAsync(user.Id, ignoreOffice: null, token: token))
            .Where(office => office.Id != officeId).ToList();

        if (assignorOfficeList.Count == 0) return Page();

        foreach (var office in assignorOfficeList)
        {
            var unassigned = await complaintService.GetUnassignedComplaintsForOfficeAsync(office.Id, token: token);
            if (unassigned.Count > 0)
                AssignorUnassignedComplaints.Add(new DashboardCard($"Unassigned Complaints in {office.Name}")
                    { Complaints = unassigned });
        }

        return Page();
    }

    public record DashboardCard(string Title)
    {
        public required IReadOnlyCollection<ComplaintSearchResultDto> Complaints { get; init; }
        public string CardId => Title.ToLowerInvariant().Replace(oldChar: ' ', newChar: '-');
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return await BuildDashboardAsync(token);

        if (!int.TryParse(FindId, out var idInt))
            ModelState.AddModelError(nameof(FindId), "Complaint ID must be a number.");
        else if (!await complaintService.ExistsAsync(idInt, token: token))
            ModelState.AddModelError(nameof(FindId), "The Complaint ID entered does not exist.");

        return ModelState.IsValid
            ? RedirectToPage("Complaints/Details", routeValues: new { id = FindId })
            : await BuildDashboardAsync(token);
    }
}
