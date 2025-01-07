using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Concerns;
using Cts.Domain.Data;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using System.ComponentModel.DataAnnotations;

namespace Cts.WebApp.Pages;

[AllowAnonymous]
public class IndexModel(IComplaintService complaints, IConcernService concerns) : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Please enter a complaint ID.")]
    [Display(Name = "Complaint ID")]
    public string? FindId { get; set; }

    public ComplaintPublicSearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<ComplaintSearchResultDto> SearchResults { get; private set; } = null!;
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());
    public SearchResultsDisplay ResultsDisplay => new(Spec, SearchResults, PaginationNav, IsPublic: true);

    public SelectList ConcernsSelectList { get; private set; } = null!;
    public static SelectList CountiesSelectList => new(Data.Counties);
    public static SelectList StatesSelectList => new(Data.States);

    public async Task OnGetAsync() => await PopulateSelectListsAsync();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        if (!int.TryParse(FindId, out var idInt))
            ModelState.AddModelError(nameof(FindId), "Complaint ID must be a number.");
        else if (!await complaints.PublicExistsAsync(idInt))
            ModelState.AddModelError(nameof(FindId),
                "The Complaint ID entered does not exist or is not publicly available.");

        if (ModelState.IsValid)
            return RedirectToPage("Complaint", routeValues: new { id = FindId });

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnGetSearchAsync(ComplaintPublicSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();
        await PopulateSelectListsAsync();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, Spec.Sort.GetDescription());
        SearchResults = await complaints.PublicSearchAsync(Spec, paging);
        ShowResults = true;
        return Page();
    }

    private async Task PopulateSelectListsAsync() =>
        ConcernsSelectList = (await concerns.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
}
