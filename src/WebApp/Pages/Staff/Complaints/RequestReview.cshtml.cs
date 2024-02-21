﻿using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Staff.Complaints;

public class RequestReviewModel(
    IComplaintService complaintService,
    IAuthorizationService authorizationService,
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

        await complaintService.RequestReviewAsync(ComplaintRequestReview);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "The Complaint has been submitted for review.");
        return RedirectToPage("Details", new { id = ComplaintRequestReview.ComplaintId });
    }

    private async Task<bool> UserCanRequestReviewAsync(ComplaintViewDto complaintView) =>
        (await authorizationService.AuthorizeAsync(User, complaintView, ComplaintOperation.RequestReview)).Succeeded;

    private async Task PopulateSelectListsAsync(Guid currentOfficeId) =>
        ReviewersSelectList = (await staffService.GetUsersInRoleAsListItemsAsync(AppRole.ManagerRole, currentOfficeId))
            .ToSelectList();
}
