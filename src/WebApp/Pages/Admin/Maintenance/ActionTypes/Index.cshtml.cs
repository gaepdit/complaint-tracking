using Cts.AppServices.ActionTypes;
using Cts.AppServices.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;

[Authorize]
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
        IsSiteMaintainer = (await authorization.AuthorizeAsync(User, nameof(Policies.SiteMaintainer))).Succeeded;
    }
}
