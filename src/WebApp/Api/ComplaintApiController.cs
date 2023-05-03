using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Cts.WebApp.Api;

[ApiController]
[Route("api/complaints")]
[Produces("application/json")]
public class ComplaintApiController : Controller
{
    private readonly IComplaintService _service;
    public ComplaintApiController(IComplaintService service) => _service = service;

    [HttpGet]
    public async Task<IPaginatedResult<ComplaintSearchResultDto>> ListComplaintsAsync(
        [FromQuery] ComplaintPublicSearchDto spec,
        [FromQuery] ushort page = 1,
        [FromQuery] ushort pageSize = 25) =>
        await _service.PublicSearchAsync(spec, new PaginatedRequest(page, pageSize));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ComplaintPublicViewDto>> GetComplaintAsync([FromRoute] int id)
    {
        var item = await _service.GetPublicAsync(id);
        return item is null ? Problem("ID not found.", statusCode: 404) : Ok(item);
    }
}
