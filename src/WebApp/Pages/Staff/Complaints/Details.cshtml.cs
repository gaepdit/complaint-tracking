﻿using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize]
public class DetailsModel : PageModel
{
    // Constructor
    private readonly IComplaintService _complaints;
    private readonly IStaffService _staff;
    private readonly IAuthorizationService _authorization;

    public DetailsModel(
        IComplaintService complaints,
        IStaffService staff,
        IAuthorizationService authorization)
    {
        _complaints = complaints;
        _staff = staff;
        _authorization = authorization;
    }

    // Properties
    public ComplaintViewDto Item { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Methods
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var staff = await _staff.GetCurrentUserAsync();
        if (staff is not { Active: true }) return Forbid();

        if (id is null) return RedirectToPage("../Index");
        var item = await _complaints.FindAsync(id.Value);
        if (item is null) return NotFound();

        item.CurrentUserOfficeId = staff.Office?.Id ?? Guid.Empty;
        Item = item;

        foreach (var operation in ComplaintOperation.AllOperations)
            await SetPermissionAsync(operation);

        if (item.IsDeleted && !UserCan[ComplaintOperation.ManageDeletions]) return Forbid();
        return Page();
    }

    private async Task SetPermissionAsync(IAuthorizationRequirement operation) =>
        UserCan[operation] = (await _authorization.AuthorizeAsync(User, Item, operation)).Succeeded;
}
