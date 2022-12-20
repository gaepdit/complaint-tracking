﻿using Cts.AppServices.Complaints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Public.Complaint;

[AllowAnonymous]
public class IndexModel : PageModel
{
    public ComplaintPublicViewDto Item { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(
        [FromServices] IComplaintAppService service,
        int? id)
    {
        if (id == null) return NotFound();
        var item = await service.GetPublicViewAsync(id.Value);
        if (item == null) return NotFound("ID not found.");

        Item = item;
        return Page();
    }
}