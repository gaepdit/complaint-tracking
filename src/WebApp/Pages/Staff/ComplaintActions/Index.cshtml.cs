using Cts.AppServices.ActionTypes;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.Staff;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace Cts.WebApp.Pages.Staff.ComplaintActions;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel(
    IActionService actionService,
    IActionTypeService actionTypeService,
    IStaffService staffService,
    IConcernService concernService,
    IOfficeService offices,
    IAuthorizationService authorization)
    : PageModel
{
    public ActionSearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public bool CanViewDeletedActions { get; private set; }
    public IPaginatedResult<ActionSearchResultDto> SearchResults { get; private set; } = null!;
    public string SortByName => Spec.Sort.ToString();
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());

    public SelectList ActionTypeSelectList { get; private set; } = null!;
    public SelectList EnteredBySelectList { get; private set; } = null!;
    public SelectList OfficesSelectList { get; private set; } = null!;
    public SelectList ConcernsSelectList { get; private set; } = null!;

    public async Task OnGetAsync()
    {
        Spec = new ActionSearchDto();
        CanViewDeletedActions = await authorization.Succeeded(User, Policies.DivisionManager);
        await PopulateSelectListsAsync();
    }

    public async Task<IActionResult> OnGetSearchAsync(ActionSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();
        CanViewDeletedActions = await authorization.Succeeded(User, Policies.DivisionManager);
        await PopulateSelectListsAsync();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, Spec.Sort.GetDescription());
        SearchResults = await actionService.SearchAsync(Spec, paging);
        ShowResults = true;
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        ActionTypeSelectList = (await actionTypeService.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        EnteredBySelectList = (await staffService.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        ConcernsSelectList = (await concernService.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        OfficesSelectList = (await offices.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
    }
}
