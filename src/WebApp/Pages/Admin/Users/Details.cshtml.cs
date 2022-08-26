using Cts.AppServices.StaffServices;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Users;

public class Details : PageModel
{
    public StaffViewDto DisplayUser { get; private set; } = default!;
    public string? OfficeName => DisplayUser.Office?.Name;
    public IList<CtsRole> Roles { get; private set; } = default!;
    public DisplayMessage? Message { get; private set; }

    public async Task<IActionResult> OnGetAsync([FromServices] IStaffAppService staffService, Guid? id)
    {
        if (id == null) return NotFound();
        var user = await staffService.FindAsync(id.Value);
        if (user == null) return NotFound("ID not found.");

        DisplayUser = user;
        Roles = await staffService.GetCtsRolesAsync(DisplayUser.Id);
        Message = TempData.GetDisplayMessage();

        return Page();
    }
}
