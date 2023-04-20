using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cts.WebApp.Api;

[Authorize]
[ApiController]
[Route("api/complaints/admin")]
[Produces("application/json")]
public class ComplaintAdminApiController : Controller
{
    private readonly IComplaintAppService _service;
    public ComplaintAdminApiController(IComplaintAppService service) => _service = service;

    [HttpGet]
    public async Task<IPaginatedResult<ComplaintSearchResultDto>> ListComplaintsAsync(
        [FromQuery] ComplaintSearchDto spec,
        [FromQuery] ushort page = 1,
        [FromQuery] ushort pageSize = 25) =>
        await _service.SearchAsync(spec, new PaginatedRequest(page, pageSize));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ComplaintViewDto>> GetComplaintAsync([FromRoute] int id)
    {
        var item = await _service.GetAsync(id);
        return item is null ? Problem("ID not found.", statusCode: 404) : Ok(item);
    }
}
