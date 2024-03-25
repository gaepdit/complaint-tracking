using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Requirements;
using Cts.AppServices.UserServices;

namespace Cts.WebApp.Api;

[ApiController]
[Route("api/offices")]
[Produces("application/json")]
public class OfficeApiController(
    IOfficeService officeService,
    IUserService userService,
    IAuthorizationService authorization) : Controller
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
        Json(await officeService.GetStaffAsListItemsAsync(id));

    [HttpGet("{id:guid}/all-staff")]
    public async Task<IActionResult> GetAllStaffAsync([FromRoute] Guid id) =>
        (await authorization.AuthorizeAsync(User, Policies.ActiveUser).ConfigureAwait(false)).Succeeded
            ? Json(await officeService.GetStaffAsListItemsAsync(id, includeInactive: true))
            : Unauthorized();

    [HttpGet("{id:guid}/staff-for-assignment")]
    public async Task<IActionResult> GetStaffForAssignmentAsync([FromRoute] Guid id)
    {
        if (!(await authorization.AuthorizeAsync(User, Policies.ActiveUser).ConfigureAwait(false)).Succeeded)
            return Unauthorized();

        var resource = new OfficeAndUser(await officeService.FindAsync(id), await userService.GetCurrentUserAsync());

        return (await authorization.AuthorizeAsync(User, resource, new OfficeAssignmentRequirement())).Succeeded
            ? Json(await officeService.GetStaffAsListItemsAsync(id))
            : Json(null);
    }
}
