using Cts.AppServices.ActionTypes;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.RazorHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Pages.Admin.Maintenance.ActionTypes;

public class Index : PageModel
{
    public IReadOnlyList<ActionTypeViewDto> Items { get; private set; } = default!;
    public static MaintenanceOption ThisOption => MaintenanceOption.ActionType;
    public DisplayMessage? Message { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync([FromServices] IActionTypeAppService service)
    {
        Items = await service.GetListAsync();
        Message = TempData.GetDisplayMessage();
    }
}
