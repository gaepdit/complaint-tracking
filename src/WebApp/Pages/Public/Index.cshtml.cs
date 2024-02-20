using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto.Query;
using Cts.AppServices.Concerns;
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
using System.ComponentModel.DataAnnotations;

namespace Cts.WebApp.Pages.Public;

[AllowAnonymous]
public class IndexModel(IComplaintService complaints, IConcernService concerns) : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Please enter a complaint ID.")]
    [Display(Name = "Complaint ID")]
    public string? FindId { get; set; }

    public ComplaintPublicSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<ComplaintSearchResultDto> SearchResults { get; private set; } = default!;
    public string SortByName => Spec.Sort.ToString();
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());

    public SelectList ConcernsSelectList { get; private set; } = default!;
    public SelectList CountiesSelectList => new(Data.Counties);
    public SelectList StatesSelectList => new(Data.States);

    public Task OnGetAsync()
    {
        Spec = new ComplaintPublicSearchDto();
        return PopulateSelectListsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        if (!int.TryParse(FindId, out var idInt))
        {
            ModelState.AddModelError(nameof(FindId), "Complaint ID must be a number.");
        }
        else if (!await complaints.PublicExistsAsync(idInt))
        {
            ModelState.AddModelError(nameof(FindId),
                "The Complaint ID entered does not exist or is not publicly available. " +
                "(Complaints are only made available on this site after EPD’s investigation has concluded.) ");
        }

        if (!ModelState.IsValid) return Page();
        return RedirectToPage("Complaints/Index", new { id = FindId });
    }

    public async Task<IActionResult> OnGetSearchAsync(ComplaintPublicSearchDto spec, [FromQuery] int p = 1)
    {
        spec.TrimAll();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, spec.Sort.GetDescription());

        Spec = spec;
        ShowResults = true;

        await PopulateSelectListsAsync();
        SearchResults = await complaints.PublicSearchAsync(spec, paging);
        return Page();
    }

    private async Task PopulateSelectListsAsync() =>
        ConcernsSelectList = (await concerns.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
}
