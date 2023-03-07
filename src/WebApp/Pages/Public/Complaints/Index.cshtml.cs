using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Public.Complaints;

[AllowAnonymous]
public class IndexModel : PageModel
{
    public ComplaintPublicViewDto Item { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync([FromServices] IComplaintAppService service, int? id)
    {
        if (id == null) return RedirectToPage("../Index");
        var item = await service.GetPublicAsync(id.Value);
        if (item == null) return NotFound();

        Item = item;
        return Page();
    }
}
