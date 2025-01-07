using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using GaEpd.AppLibrary.Pagination;

namespace Cts.WebApp.Api;

[ApiController]
[Route("api/complaints")]
[Produces("application/json")]
public class ComplaintApiController(IComplaintService complaintService) : ControllerBase
{
    [HttpGet]
    public async Task<IPaginatedResult<ComplaintSearchResultDto>> ListComplaintsAsync(
        [FromQuery] ComplaintPublicSearchDto spec,
        [FromQuery] ushort page = 1,
        [FromQuery] ushort pageSize = 25) =>
        await complaintService.PublicSearchAsync(spec, new PaginatedRequest(page, pageSize));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ComplaintPublicViewDto>> GetComplaintAsync([FromRoute] int id)
    {
        var item = await complaintService.FindPublicAsync(id);
        return item is null ? Problem("ID not found.", statusCode: 404) : Ok(item);
    }
}
