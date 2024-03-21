﻿using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using System.ComponentModel.DataAnnotations;

namespace Cts.WebApp.Pages.Staff;

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

    public async Task OnGetAsync(CancellationToken token)
    {
        var user = await staffService.GetCurrentUserAsync();
        var officeName = user.Office?.Name;
        var officeId = user.Office?.Id ?? Guid.Empty;

        IsStaff = (await authorization.AuthorizeAsync(User, Policies.StaffUser)).Succeeded;
        if (IsStaff)
        {
            MyNewComplaints = new DashboardCard("My new complaints")
                { Complaints = await complaintService.GetNewComplaintsForUserAsync(user.Id, token) };
            MyOpenComplaints = new DashboardCard("My open complaints")
                { Complaints = await complaintService.GetOpenComplaintsForUserAsync(user.Id, token) };
        }

        IsManager = (await authorization.AuthorizeAsync(User, Policies.Manager)).Succeeded;
        if (IsManager)
        {
            MgrReviewPending = new DashboardCard("My complaints to review")
                { Complaints = await complaintService.GetReviewPendingComplaintsForUserAsync(user.Id, token) };
            MgrUnassignedComplaints = new DashboardCard($"Complaints in {officeName} that have not been assigned")
                { Complaints = await complaintService.GetUnassignedComplaintsForOfficeAsync(officeId, token) };
            MgrUnacceptedComplaints =
                new DashboardCard($"Complaints in {officeName} that have been assigned but not accepted")
                    { Complaints = await complaintService.GetUnacceptedComplaintsForOfficeAsync(officeId, token) };
        }

        var assignorOfficeList = (await officeService.GetOfficesForAssignorAsync(user.Id, ignoreOffice: null, token))
            .Where(office => office.Id != officeId).ToList();

        if (assignorOfficeList.Count > 0)
        {
            foreach (var office in assignorOfficeList)
            {
                var unassigned = await complaintService.GetUnassignedComplaintsForOfficeAsync(office.Id, token);
                if (unassigned.Count > 0)
                    AssignorUnassignedComplaints.Add(new DashboardCard($"Unassigned Complaints in {office.Name}")
                        { Complaints = unassigned });
            }
        }
    }

    public record DashboardCard(string Title)
    {
        public required IReadOnlyCollection<ComplaintSearchResultDto> Complaints { get; init; }
        public string CardId => Title.ToLowerInvariant().Replace(oldChar: ' ', newChar: '-');
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        if (!int.TryParse(FindId, out var idInt))
        {
            ModelState.AddModelError(nameof(FindId), "Complaint ID must be a number.");
        }
        else if (!await complaintService.ExistsAsync(idInt))
        {
            ModelState.AddModelError(nameof(FindId), "The Complaint ID entered does not exist.");
        }

        if (!ModelState.IsValid) return Page();
        return RedirectToPage("Complaints/Details", new { id = FindId });
    }
}