using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Data;
using Cts.Domain.ValueObjects;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageDisplayHelpers;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Admin.Complaints;

[Authorize]
public class AddModel : PageModel
{
    // Properties
    [BindProperty]
    public ComplaintCreateDto Item { get; set; } = default!;

    // Select lists
    public SelectList ActiveStaffMembers { get; private set; } = default!;
    public SelectList ConcernsSelectList { get; private set; } = default!;
    public SelectList OfficesSelectList { get; private set; } = default!;
    public SelectList StatesSelectList => new(Data.States);
    public SelectList CountiesSelectList => new(Data.Counties);

    // Services
    private readonly IComplaintAppService _complaints;
    private readonly IStaffAppService _staff;
    private readonly IConcernAppService _concernService;
    private readonly IOfficeAppService _offices;

    public AddModel(
        IComplaintAppService service,
        IStaffAppService staff,
        IConcernAppService concerns,
        IOfficeAppService offices)
    {
        _complaints = service;
        _staff = staff;
        _concernService = concerns;
        _offices = offices;
    }

    public async Task OnGetAsync()
    {
        await PopulateSelectListsAsync();
        var user = await _staff.GetCurrentUserAsync();
        Item = new ComplaintCreateDto("Georgia", user?.Id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        var id = await _complaints.CreateAsync(Item);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint successfully created.");
        return RedirectToPage("./Details", new { id });
    }

    private async Task PopulateSelectListsAsync()
    {

        ActiveStaffMembers = (await _staff.GetActiveStaffMembersAsync()).ToSelectList();
        ConcernsSelectList = (await _concernService.GetActiveListItemsAsync()).ToSelectList();
        OfficesSelectList = (await _offices.GetActiveListItemsAsync()).ToSelectList();
    }
}
