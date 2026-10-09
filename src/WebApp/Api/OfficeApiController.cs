using Cts.AppServices.AuthorizationPolicies;
using Cts.AppServices.AuthorizationPolicies.Requirements;
using Cts.AppServices.Offices;

namespace Cts.WebApp.Api;

[ApiController]
[Route("api/offices/{id:guid}")]
[Produces("application/json")]
public class OfficeApiController(IOfficeService officeService, IAuthorizationService authorization) : ControllerBase
{
    [HttpGet("all-staff")]
    public async Task<IActionResult> GetAllStaffAsync([FromRoute] Guid id) =>
        await authorization.Succeeded(User, Policies.ActiveUser)
            ? new JsonResult(await officeService.GetStaffAsListItemsAsync(id, includeInactive: true))
            : Unauthorized();

    [HttpGet("staff-for-assignment")]
    public async Task<IActionResult> GetStaffForAssignmentAsync([FromRoute] Guid id)
    {
        if (!await authorization.Succeeded(User, Policies.ActiveUser)) return Unauthorized();

        var office = await officeService.FindAsync(id);

        return new JsonResult(await authorization.Succeeded(User, office, new OfficeAssignmentRequirement())
            ? await officeService.GetStaffAsListItemsAsync(id)
            : null);
    }
}
