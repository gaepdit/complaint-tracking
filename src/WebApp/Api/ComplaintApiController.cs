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
    private readonly IComplaintAppService _service;
    public ComplaintApiController(IComplaintAppService service) => _service = service;

    [HttpGet]
    public async Task<IPaginatedResult<ComplaintSearchResultDto>> ListComplaintsAsync(
        [FromQuery] ComplaintPublicSearchDto spec, [FromQuery] ushort page = 1, [FromQuery] ushort pageSize = 25) =>
        await _service.PublicSearchAsync(spec, new PaginatedRequest(page, pageSize));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ComplaintPublicViewDto>> GetComplaintAsync([FromRoute] int id)
    {
        var item = await _service.GetPublicAsync(id);
        return item != null ? Ok(item) : Problem("ID not found.", statusCode: 404);
    }
}
