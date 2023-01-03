using Cts.AppServices.Concerns;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageDisplayHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Maintenance.Concerns;

[Authorize]
public class IndexModel : PageModel
{
    public IReadOnlyList<ConcernViewDto> Items { get; private set; } = default!;
    public static MaintenanceOption ThisOption => MaintenanceOption.Concern;
    public DisplayMessage? Message { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync([FromServices] IConcernAppService service)
    {
        Items = await service.GetListAsync();
        Message = TempData.GetDisplayMessage();
    }
}
