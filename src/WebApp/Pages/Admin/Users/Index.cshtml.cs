using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel : PageModel
{
    // Constructor
    private readonly IOfficeService _officeService;
    private readonly IStaffService _staffService;

    public IndexModel(
        IOfficeService officeService,
        IStaffService staffService)
    {
        _officeService = officeService;
        _staffService = staffService;
    }

    // Properties
    public StaffSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<StaffSearchResultDto> SearchResults { get; private set; } = default!;
    public string SortByName => Spec.Sort.ToString();
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());

    // Select lists
    public SelectList RoleItems { get; private set; } = default!;
    public SelectList OfficeItems { get; private set; } = default!;

    // Methods
    public Task OnGetAsync() => PopulateSelectListsAsync();

    public async Task<IActionResult> OnGetSearchAsync(StaffSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();
        await PopulateSelectListsAsync();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, Spec.Sort.GetDescription());
        SearchResults = await _staffService.SearchAsync(Spec, paging);
        ShowResults = true;
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        OfficeItems = (await _officeService.GetActiveListItemsAsync()).ToSelectList();
        RoleItems = AppRole.AllRoles
            .Select(r => new ListItem<string>(r.Key, r.Value.DisplayName))
            .ToSelectList();
    }
}
