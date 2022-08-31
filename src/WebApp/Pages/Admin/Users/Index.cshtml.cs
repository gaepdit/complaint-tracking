﻿using Cts.AppServices.Offices;
using Cts.AppServices.StaffServices;
using Cts.Domain.Identity;
using GaEpd.Library.ListItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Admin.Users;

public class IndexModel : PageModel
{
    private readonly IOfficeAppService _officeService;
    private readonly IStaffAppService _staffService;

    public IndexModel(IOfficeAppService officeService, IStaffAppService staffService)
    {
        _officeService = officeService;
        _staffService = staffService;
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
        SearchResults = await _staffService.GetListAsync(filter);
        ShowResults = true;
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        OfficeItems = (await _officeService.GetActiveListItemsAsync()).ToSelectList();
        RoleItems = CtsRole.AllRolesList.ToSelectList();
    }
}
