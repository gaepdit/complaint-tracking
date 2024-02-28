using Cts.AppServices.ActionTypes;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Concerns;
using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace Cts.WebApp.Pages.Staff.ComplaintActions;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel(
    IComplaintActionService complaintActionService,
    IActionTypeService actionTypeService,
    IStaffService staffService,
    IConcernService concernService,
    IAuthorizationService authorization)
    : PageModel
{
    public ComplaintActionSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public bool CanViewDeletedActions { get; private set; }
    public IPaginatedResult<ComplaintActionSearchResultDto> SearchResults { get; private set; } = default!;
    public string SortByName => Spec.Sort.ToString();
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());

    public SelectList ActionTypeSelectList { get; private set; } = default!;
    public SelectList EnteredBySelectList { get; private set; } = default!;
    public SelectList ConcernsSelectList { get; private set; } = default!;
    
    public async Task OnGetAsync()
    {
        Spec = new ComplaintActionSearchDto();
        CanViewDeletedActions = (await authorization.AuthorizeAsync(User, nameof(Policies.DivisionManager))).Succeeded;
        await PopulateSelectListsAsync();
    }

    public async Task<IActionResult> OnGetSearchAsync(ComplaintActionSearchDto spec, [FromQuery] int p = 1)
    {
        spec.TrimAll();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, spec.Sort.GetDescription());
        CanViewDeletedActions = (await authorization.AuthorizeAsync(User, nameof(Policies.DivisionManager))).Succeeded;
        if (!CanViewDeletedActions) spec.DeletedStatus = null;

        Spec = spec;
        ShowResults = true;

        await PopulateSelectListsAsync();
        SearchResults = await complaintActionService.SearchAsync(spec, paging);
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        ActionTypeSelectList= (await actionTypeService.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        EnteredBySelectList = (await staffService.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        ConcernsSelectList = (await concernService.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
    }
}
