using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;

namespace Cts.WebApp.Pages;

[AllowAnonymous]
public class ComplaintModel(
    [FromServices] IComplaintService service,
    [FromServices] IAuthorizationService authorization) : PageModel
{
    public ComplaintPublicViewDto Item { get; private set; } = default!;
    public bool UserIsActive { get; private set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var item = await service.FindPublicAsync(id.Value);
        if (item is null) return NotFound();

        UserIsActive = await authorization.Succeeded(User, Policies.ActiveUser);
        Item = item;
        return Page();
    }
}
