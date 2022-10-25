﻿using Cts.AppServices.Offices;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize]
public class IndexModel : PageModel
{
    public IReadOnlyList<OfficeViewDto> Items { get; private set; } = default!;
    public static MaintenanceOption ThisOption => MaintenanceOption.Office;
    public DisplayMessage? Message { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync([FromServices] IOfficeAppService service)
    {
        Items = await service.GetListAsync();
        Message = TempData.GetDisplayMessage();
    }
}