﻿using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Public.Complaints;

[AllowAnonymous]
public class IndexModel : PageModel
{
    public ComplaintPublicViewDto Item { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(
        [FromServices] IComplaintService service, 
        int? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var item = await service.FindPublicAsync(id.Value);
        if (item is null) return NotFound();

        Item = item;
        return Page();
    }
}
