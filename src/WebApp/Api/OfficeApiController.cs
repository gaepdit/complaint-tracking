using Cts.AppServices.Offices;
using Microsoft.AspNetCore.Mvc;

namespace Cts.WebApp.Api;

[ApiController]
[Route("api/offices")]
[Produces("application/json")]
public class OfficeApiController : Controller
{
    private readonly IOfficeAppService _officeAppService;
    public OfficeApiController(IOfficeAppService officeAppService) => _officeAppService = officeAppService;

    [HttpGet]
    public async Task<IReadOnlyList<OfficeAdminViewDto>> ListOfficesAsync() =>
        await _officeAppService.GetListAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OfficeAdminViewDto>> GetOfficeAsync([FromRoute] Guid id)
    {
        var item = await _officeAppService.FindAsync(id);
        return item != null ? Ok(item) : Problem("ID not found.", statusCode: 404);
    }
}
