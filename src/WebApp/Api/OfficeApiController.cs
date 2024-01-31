using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.AppServices.UserServices;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cts.WebApp.Api;

[ApiController]
[Route("api/offices")]
[Produces("application/json")]
public class OfficeApiController(IOfficeService officeService, IStaffService staffService, IUserService userService)
    : Controller
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
    public async Task<IActionResult> GetAllStaffAsync([FromRoute] Guid id)
    {
        var user = await userService.GetCurrentUserAsync();
        if (user is null || !user.Active) return Unauthorized();
        return Json(await officeService.GetStaffAsListItemsAsync(id, includeInactive: true));
    }

    [HttpGet("{id:guid}/staff-for-assignment")]
    public async Task<IActionResult> GetStaffForAssignmentAsync([FromRoute] Guid id)
    {
        var user = await userService.GetCurrentUserAsync();
        if (user is null || !user.Active) return Unauthorized();

        if (user.Office?.Id == id // user is in this office
            || await officeService.UserIsAssignorAsync(id, user.Id) // user is assignor for this office
            || await staffService.HasAppRoleAsync(user.Id, AppRole.DivisionManagerRole)) // user is Division Manager
        {
            return Json(await officeService.GetStaffAsListItemsAsync(id));
        }

        return Json(null);
    }
}
