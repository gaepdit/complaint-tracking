using Cts.AppServices.AuthorizationPolicies;
using Cts.AppServices.AuthorizationPolicies.Requirements;
using Cts.AppServices.Offices;

namespace Cts.WebApp.Pages.Content;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class OfficeModel(IOfficeService officeService, IAuthorizationService authorization) : PageModel
{
    [FromRoute] public Guid Id { get; set; }

    public NotFoundResult OnGet() => NotFound();

    public async Task<JsonResult> OnGetStaffAsync() =>
        new(await officeService.GetStaffAsListItemsAsync(Id, includeInactive: true));

    public async Task<JsonResult> OnGetStaffForAssignmentAsync()
    {
        var office = await officeService.FindAsync(Id);

        var results = await authorization.Succeeded(User, office, new OfficeAssignmentRequirement())
            ? await officeService.GetStaffAsListItemsAsync(Id)
            : null;

        return new JsonResult(results);
    }
}
