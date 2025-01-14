using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;

namespace Cts.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel : PageModel
{
    public IReadOnlyList<OfficeWithAssignorDto> Items { get; private set; } = null!;
    public static MaintenanceOption ThisOption => MaintenanceOption.Office;
    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync(
        [FromServices] IOfficeService service,
        [FromServices] IAuthorizationService authorization)
    {
        Items = await service.GetListIncludeAssignorAsync();
        IsSiteMaintainer = await authorization.Succeeded(User, Policies.SiteMaintainer);
    }
}
