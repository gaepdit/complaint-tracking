using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize]
public class DetailsModel(IComplaintService complaints, IStaffService staffService, IAuthorizationService authorization)
    : PageModel
{
    public ComplaintViewDto Item { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var staff = await staffService.GetCurrentUserAsync();
        if (staff is not { Active: true }) return Forbid();

        if (id is null) return RedirectToPage("../Index");
        var item = await complaints.FindAsync(id.Value);
        if (item is null) return NotFound();

        item.CurrentUserOfficeId = staff.Office?.Id ?? Guid.Empty;
        Item = item;

        foreach (var operation in ComplaintOperation.AllOperations)
            await SetPermissionAsync(operation);

        if (item.IsDeleted && !UserCan[ComplaintOperation.ManageDeletions]) return Forbid();
        return Page();
    }

    private async Task SetPermissionAsync(IAuthorizationRequirement operation) =>
        UserCan[operation] = (await authorization.AuthorizeAsync(User, Item, operation)).Succeeded;
}
