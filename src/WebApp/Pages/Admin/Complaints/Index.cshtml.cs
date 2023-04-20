using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Data;
using Cts.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Enums;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Admin.Complaints;

[Authorize]
public class IndexModel : PageModel
{
    // Properties
    public ComplaintSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<ComplaintSearchResultDto> Results { get; private set; } = default!;
    public string SortByName => Spec.Sort.ToString();

    // Select Lists
    public SelectList ReceivedBySelectList { get; private set; } = default!;
    public SelectList ConcernsSelectList { get; private set; } = default!;
    public SelectList OfficesSelectList { get; set; } = default!;
    public SelectList AssignedToSelectList { get; set; } = default!;

    public SelectList CountiesSelectList => new(Data.Counties);
    public SelectList StatesSelectList => new(Data.States);

    // Services
    private readonly IComplaintAppService _complaints;
    private readonly IStaffAppService _staff;
    private readonly IConcernAppService _concerns;
    private readonly IOfficeAppService _offices;

    public IndexModel(
        IComplaintAppService complaints,
        IStaffAppService staff,
        IConcernAppService concerns,
        IOfficeAppService offices)
    {
        _complaints = complaints;
        _staff = staff;
        _concerns = concerns;
        _offices = offices;
    }

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
        Results = await _complaints.SearchAsync(spec, paging);
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        ReceivedBySelectList = (await _staff.GetStaffListItemsAsync(false)).ToSelectList();
        ConcernsSelectList = (await _concerns.GetActiveListItemsAsync()).ToSelectList();
        OfficesSelectList = (await _offices.GetActiveListItemsAsync()).ToSelectList();
        AssignedToSelectList = (await _offices.GetStaffListItemsAsync(Spec.Office, false)).ToSelectList();
    }
}
