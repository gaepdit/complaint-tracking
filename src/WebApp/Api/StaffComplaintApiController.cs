using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cts.WebApp.Api;

[Authorize]
[ApiController]
[Route("api/staff/complaints")]
[Produces("application/json")]
public class StaffComplaintApiController(IComplaintService complaintService) : Controller
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
