using Microsoft.AspNetCore.Mvc;
using MyAppRoot.AppServices.Offices;

namespace MyAppRoot.WebApp.Api;

[ApiController]
[Route("api/office")]
[Produces("application/json")]
public class OfficeApiController : Controller
{
    private readonly IOfficeAppService _officeAppService;
    public OfficeApiController(IOfficeAppService officeAppService) => _officeAppService = officeAppService;

    [HttpGet]
    public async Task<IReadOnlyList<OfficeViewDto>> ListOfficesAsync() =>
        (await _officeAppService.GetListAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OfficeViewDto>> GetOfficeAsync([FromRoute] Guid id)
    {
        var item = await _officeAppService.FindAsync(id);
        return item != null ? Ok(item) : Problem("ID not found.", statusCode: 404);
    }
}
