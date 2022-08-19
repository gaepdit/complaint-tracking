using Cts.AppServices.Offices;
using Cts.AppServices.StaffServices;
using Cts.Domain.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Admin.Users;

public class IndexModel : PageModel
{
    private readonly IOfficeAppService _officeService;
    private readonly IStaffAppService _userService;

    public IndexModel(IOfficeAppService officeService, IStaffAppService userService)
    {
        _officeService = officeService;
        _userService = userService;
    }

    public StaffSearchDto Filter { get; set; } = default!;
    public SelectList RoleItems { get; private set; } = default!;
    public SelectList OfficeItems { get; private set; } = default!;
    public bool ShowResults { get; private set; }
    public List<StaffViewDto> SearchResults { get; private set; } = default!;

    [TempData]
    public Guid? HighlightId { get; set; }

    public Task OnGetAsync() => PopulateSelectListsAsync();

    public async Task<IActionResult> OnGetSearchAsync(StaffSearchDto filter)
    {
        await PopulateSelectListsAsync();
        filter.TrimAll();
        Filter = filter;
        if (!ModelState.IsValid) return Page();
        SearchResults = await _userService.FindUsersAsync(filter);
        ShowResults = true;
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        OfficeItems = new SelectList(await _officeService.GetListAsync(),
            nameof(OfficeViewDto.Id), nameof(OfficeViewDto.Name));
        RoleItems = new SelectList(CtsRole.AllRoles,
            nameof(CtsRole.Name), nameof(CtsRole.DisplayName));
    }
}
