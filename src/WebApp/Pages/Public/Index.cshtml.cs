using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Concerns;
using Cts.Domain.Data;
using Cts.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Enums;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Cts.WebApp.Pages.Public;

[AllowAnonymous]
public class IndexModel : PageModel
{
    // Constructor
    private readonly IComplaintAppService _complaints;
    private readonly IConcernAppService _concerns;

    public IndexModel(
        IComplaintAppService complaints,
        IConcernAppService concerns)
    {
        _complaints = complaints;
        _concerns = concerns;
    }

    // Properties
    [BindProperty]
    [Required(ErrorMessage = "Please enter a complaint ID.")]
    [Display(Name = "Complaint ID")]
    public string? FindId { get; set; }

    public ComplaintPublicSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<ComplaintSearchResultDto> Results { get; private set; } = default!;
    public string SortByName => Spec.Sort.ToString();

    // Select lists
    public SelectList ConcernsSelectList { get; private set; } = default!;
    public SelectList CountiesSelectList => new(Data.Counties);
    public SelectList StatesSelectList => new(Data.States);

    // Methods
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
        else if (!await _complaints.PublicExistsAsync(idInt))
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
        Results = await _complaints.PublicSearchAsync(spec, paging);
        return Page();
    }

    private async Task PopulateSelectListsAsync() =>
        ConcernsSelectList = (await _concerns.GetActiveListItemsAsync()).ToSelectList();
}
