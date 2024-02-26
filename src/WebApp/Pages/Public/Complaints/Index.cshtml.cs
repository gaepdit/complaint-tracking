using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Public.Complaints;

[AllowAnonymous]
public class IndexModel([FromServices] IComplaintService service, [FromServices] IAuthorizationService authorization)
    : PageModel
{
    public ComplaintPublicViewDto Item { get; private set; } = default!;
    public bool UserIsActive { get; private set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var item = await service.FindPublicAsync(id.Value);
        if (item is null) return NotFound();

        UserIsActive = (await authorization.AuthorizeAsync(User, Policies.ActiveUser)).Succeeded;
        Item = item;
        return Page();
    }
}
