using Cts.AppServices.ActionTypes;
using Cts.AppServices.Permissions;

namespace Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel : PageModel
{
    public IReadOnlyList<ActionTypeViewDto> Items { get; private set; } = default!;
    public static MaintenanceOption ThisOption => MaintenanceOption.ActionType;
    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync(
        [FromServices] IActionTypeService service,
        [FromServices] IAuthorizationService authorization)
    {
        Items = await service.GetListAsync();
        IsSiteMaintainer = (await authorization.AuthorizeAsync(User, Policies.SiteMaintainer)).Succeeded;
    }
}
