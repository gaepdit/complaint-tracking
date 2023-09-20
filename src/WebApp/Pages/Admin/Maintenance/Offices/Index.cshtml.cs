using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize(Policy = nameof(Policies.AdministrationView))]
public class IndexModel : PageModel
{
    public IReadOnlyList<OfficeAdminViewDto> Items { get; private set; } = default!;
    public static MaintenanceOption ThisOption => MaintenanceOption.Office;
    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync(
        [FromServices] IOfficeService service,
        [FromServices] IAuthorizationService authorization)
    {
        Items = await service.GetListAsync();
        IsSiteMaintainer = (await authorization.AuthorizeAsync(User, nameof(Policies.SiteMaintainer))).Succeeded;
    }
}
