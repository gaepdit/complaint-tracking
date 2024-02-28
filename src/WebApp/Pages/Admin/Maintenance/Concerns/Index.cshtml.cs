using Cts.AppServices.Concerns;
using Cts.AppServices.Permissions;

namespace Cts.WebApp.Pages.Admin.Maintenance.Concerns;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel : PageModel
{
    public IReadOnlyList<ConcernViewDto> Items { get; private set; } = default!;
    public static MaintenanceOption ThisOption => MaintenanceOption.Concern;
    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync(
        [FromServices] IConcernService service,
        [FromServices] IAuthorizationService authorization)
    {
        Items = await service.GetListAsync();
        IsSiteMaintainer = (await authorization.AuthorizeAsync(User, Policies.SiteMaintainer)).Succeeded;
    }
}
