using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.AppServices.UserServices;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cts.WebApp.Api;

[ApiController]
[Route("api/offices")]
[Produces("application/json")]
public class OfficeApiController : Controller
{
    private readonly IOfficeService _office;
    private readonly IStaffService _staff;
    private readonly IUserService _user;

    public OfficeApiController(IOfficeService office, IStaffService staff, IUserService user)
    {
        _office = office;
        _staff = staff;
        _user = user;
    }

    [HttpGet]
    public async Task<IReadOnlyList<OfficeWithAssignorDto>> ListOfficesAsync() =>
        await _office.GetListAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OfficeWithAssignorDto>> GetOfficeAsync([FromRoute] Guid id)
    {
        var item = await _office.FindAsync(id);
        return item is null ? Problem("ID not found.", statusCode: 404) : Ok(item);
    }

    [HttpGet("{id:guid}/staff")]
    public async Task<JsonResult> GetStaffAsync([FromRoute] Guid id) =>
        Json(await _office.GetStaffListItemsAsync(id));

    [HttpGet("{id:guid}/all-staff")]
    public async Task<IActionResult> GetAllStaffAsync([FromRoute] Guid id)
    {
        var user = await _user.GetCurrentUserAsync();
        if (user is null || !user.Active) return Unauthorized();
        return Json(await _office.GetStaffListItemsAsync(id, true));
    }

    [HttpGet("{id:guid}/staff-for-assignment")]
    public async Task<IActionResult> GetStaffForAssignmentAsync([FromRoute] Guid id)
    {
        var user = await _user.GetCurrentUserAsync();
        if (user is null || !user.Active) return Unauthorized();

        if (user.Office?.Id == id // user is in this office
            || await _office.UserIsAssignorAsync(id, user.Id) // user is assignor for this office
            || await _staff.HasAppRoleAsync(user.Id, AppRole.DivisionManagerRole)) // user is Division Manager
        {
            return Json(await _office.GetStaffListItemsAsync(id));
        }

        return Json(null);
    }
}
