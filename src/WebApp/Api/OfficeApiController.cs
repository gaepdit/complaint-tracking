using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.Permissions.Requirements;

namespace Cts.WebApp.Api;

[ApiController]
[Route("api/offices")]
[Produces("application/json")]
public class OfficeApiController(
    IOfficeService officeService,
    IAuthorizationService authorization) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<OfficeWithAssignorDto>> ListOfficesAsync() =>
        await officeService.GetListIncludeAssignorAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OfficeWithAssignorDto>> GetOfficeAsync([FromRoute] Guid id)
    {
        var item = await officeService.FindAsync(id);
        return item is null ? Problem("ID not found.", statusCode: 404) : Ok(item);
    }

    [HttpGet("{id:guid}/staff")]
    public async Task<JsonResult> GetStaffAsync([FromRoute] Guid id) =>
        new(await officeService.GetStaffAsListItemsAsync(id));

    [HttpGet("{id:guid}/all-staff")]
    public async Task<IActionResult> GetAllStaffAsync([FromRoute] Guid id) =>
        await authorization.Succeeded(User, Policies.ActiveUser)
            ? new JsonResult(await officeService.GetStaffAsListItemsAsync(id, includeInactive: true))
            : Unauthorized();

    [HttpGet("{id:guid}/staff-for-assignment")]
    public async Task<IActionResult> GetStaffForAssignmentAsync([FromRoute] Guid id)
    {
        if (!await authorization.Succeeded(User, Policies.ActiveUser)) return Unauthorized();

        var office = await officeService.FindAsync(id);

        return new JsonResult(await authorization.Succeeded(User, office, new OfficeAssignmentRequirement())
            ? await officeService.GetStaffAsListItemsAsync(id)
            : null);
    }
}
