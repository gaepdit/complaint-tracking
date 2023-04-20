﻿using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cts.WebApp.Api;

[ApiController]
[Route("api/offices")]
[Produces("application/json")]
public class OfficeApiController : Controller
{
    private readonly IOfficeAppService _office;
    private readonly IStaffAppService _staff;

    public OfficeApiController(IOfficeAppService office, IStaffAppService staff)
    {
        _office = office;
        _staff = staff;
    }

    [HttpGet]
    public async Task<IReadOnlyList<OfficeAdminViewDto>> ListOfficesAsync() =>
        await _office.GetListAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OfficeAdminViewDto>> GetOfficeAsync([FromRoute] Guid id)
    {
        var item = await _office.FindAsync(id);
        return item is null ? Problem("ID not found.", statusCode: 404) : Ok(item);
    }

    [HttpGet("{id:guid}/staff")]
    public async Task<JsonResult> GetStaffAsync([FromRoute] Guid id) =>
        Json(await _office.GetStaffListItemsAsync(id, false));

    [HttpGet("{id:guid}/staff-for-assignment")]
    public async Task<JsonResult> GetStaffForAssignmentAsync([FromRoute] Guid id)
    {
        var user = await _staff.FindCurrentUserAsync();
        if (user is null || !user.Active) return Json(null);

        if (user.Office?.Id == id // user is in this office
            || await _office.UserIsAssignorAsync(id, user.Id) // user is assignor for this office
            || await _staff.HasAppRoleAsync(user.Id, AppRole.DivisionManagerRole)) // user is Division Manager
        {
            return Json(await _office.GetStaffListItemsAsync(id, true));
        }

        return Json(null);
    }
}
