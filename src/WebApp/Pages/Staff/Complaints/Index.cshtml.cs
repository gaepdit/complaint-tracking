using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.Staff;
using Cts.Domain.Data;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel(
    IComplaintService complaints,
    IStaffService staff,
    IConcernService concerns,
    IOfficeService offices,
    IAuthorizationService authorization)
    : PageModel
{
    public ComplaintSearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public bool CanViewDeletedComplaints { get; private set; }
    public IPaginatedResult<ComplaintSearchResultDto> SearchResults { get; private set; } = null!;
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());
    public SearchResultsDisplay ResultsDisplay => new(Spec, SearchResults, PaginationNav, IsPublic: false);

    public SelectList ReceivedBySelectList { get; private set; } = null!;
    public SelectList ConcernsSelectList { get; private set; } = null!;
    public SelectList OfficesSelectList { get; set; } = null!;
    public SelectList AssignedToSelectList { get; set; } = null!;

    public static SelectList CountiesSelectList => new(Data.Counties);
    public static SelectList StatesSelectList => new(Data.States);

    public async Task OnGetAsync()
    {
        Spec = new ComplaintSearchDto();
        CanViewDeletedComplaints = await authorization.Succeeded(User, Policies.DivisionManager);
        await PopulateSelectListsAsync();
    }

    public async Task<IActionResult> OnGetSearchAsync(ComplaintSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();
        CanViewDeletedComplaints = await authorization.Succeeded(User, Policies.DivisionManager);
        await PopulateSelectListsAsync();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, Spec.Sort.GetDescription());
        SearchResults = await complaints.SearchAsync(Spec, paging);
        ShowResults = true;
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        ReceivedBySelectList = (await staff.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        ConcernsSelectList = (await concerns.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        OfficesSelectList = (await offices.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        AssignedToSelectList =
            (await offices.GetStaffAsListItemsAsync(Spec.Office, includeInactive: true)).ToSelectList();
    }
}
