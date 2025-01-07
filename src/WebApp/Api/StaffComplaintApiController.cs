using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using GaEpd.AppLibrary.Pagination;

namespace Cts.WebApp.Api;

[Authorize(Policy = nameof(Policies.ActiveUser))]
[ApiController]
[Route("api/staff/complaints")]
[Produces("application/json")]
public class StaffComplaintApiController(IComplaintService complaintService) : ControllerBase
{
    [HttpGet]
    public async Task<IPaginatedResult<ComplaintSearchResultDto>> ListComplaintsAsync(
        [FromQuery] ComplaintSearchDto spec,
        [FromQuery] ushort page = 1,
        [FromQuery] ushort pageSize = 25) =>
        await complaintService.SearchAsync(spec, new PaginatedRequest(page, pageSize));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ComplaintViewDto>> GetComplaintAsync([FromRoute] int id)
    {
        var item = await complaintService.FindAsync(id);
        return item is null ? Problem("ID not found.", statusCode: 404) : Ok(item);
    }
}
