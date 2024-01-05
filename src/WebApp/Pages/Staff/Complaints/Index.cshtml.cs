using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Data;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize]
public class IndexModel(
    IComplaintService complaints,
    IStaffService staff,
    IConcernService concerns,
    IOfficeService offices)
    : PageModel
{
    public ComplaintSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<ComplaintSearchResultDto> SearchResults { get; private set; } = default!;
    public string SortByName => Spec.Sort.ToString();
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());

    public SelectList ReceivedBySelectList { get; private set; } = default!;
    public SelectList ConcernsSelectList { get; private set; } = default!;
    public SelectList OfficesSelectList { get; set; } = default!;
    public SelectList AssignedToSelectList { get; set; } = default!;

    public SelectList CountiesSelectList => new(Data.Counties);
    public SelectList StatesSelectList => new(Data.States);

    public Task OnGetAsync()
    {
        Spec = new ComplaintSearchDto();
        return PopulateSelectListsAsync();
    }

    public async Task<IActionResult> OnGetSearchAsync(ComplaintSearchDto spec, [FromQuery] int p = 1)
    {
        spec.TrimAll();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, spec.Sort.GetDescription());

        Spec = spec;
        ShowResults = true;

        await PopulateSelectListsAsync();
        SearchResults = await complaints.SearchAsync(spec, paging);
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        ReceivedBySelectList = (await staff.GetStaffListItemsAsync(true)).ToSelectList();
        ConcernsSelectList = (await concerns.GetActiveListItemsAsync()).ToSelectList();
        OfficesSelectList = (await offices.GetActiveListItemsAsync()).ToSelectList();
        AssignedToSelectList = (await offices.GetStaffListItemsAsync(Spec.Office, true)).ToSelectList();
    }
}
