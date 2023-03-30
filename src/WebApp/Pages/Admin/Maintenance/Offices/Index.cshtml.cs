using Cts.AppServices.Offices;
using Cts.AppServices.Security;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize]
public class IndexModel : PageModel
{
    public IReadOnlyList<OfficeAdminViewDto> Items { get; private set; } = default!;
    public static MaintenanceOption ThisOption => MaintenanceOption.Office;
    public DisplayMessage? Message { get; private set; }
    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync([FromServices] IOfficeAppService service,
        [FromServices] IAuthorizationService authorization)
    {
        Items = await service.GetListAsync();
        Message = TempData.GetDisplayMessage();
        IsSiteMaintainer = (await authorization.AuthorizeAsync(User, PolicyName.SiteMaintainer)).Succeeded;
    }
}
