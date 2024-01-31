using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using Cts.Domain.Data;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class AddModel(
    IComplaintService complaintService,
    IStaffService staffService,
    IConcernService concernService,
    IOfficeService officeService,
    IValidator<ComplaintCreateDto> validator)
    : PageModel
{
    [BindProperty]
    public ComplaintCreateDto Item { get; set; } = default!;

    public SelectList ConcernsSelectList { get; private set; } = default!;
    public SelectList OfficesSelectList { get; private set; } = default!;
    public SelectList AllActiveStaffSelectList { get; private set; } = default!;
    public SelectList ActiveStaffInOfficeSelectList { get; private set; } = default!;

    public SelectList StatesSelectList => new(Data.States);
    public SelectList CountiesSelectList => new(Data.Counties);

    public async Task OnGetAsync()
    {
        var user = await staffService.GetCurrentUserAsync();
        Item = new ComplaintCreateDto(user.Id, user.Office?.Id);
        await PopulateSelectListsAsync(user.Office?.Id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(Item.CurrentOfficeId);
            return Page();
        }

        var id = await complaintService.CreateAsync(Item);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint successfully created.");
        return RedirectToPage("Details", new { id });
    }

    private async Task PopulateSelectListsAsync(Guid? currentOfficeId)
    {
        ConcernsSelectList = (await concernService.GetAsListItemsAsync()).ToSelectList();
        OfficesSelectList = (await officeService.GetAsListItemsAsync()).ToSelectList();
        AllActiveStaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
        ActiveStaffInOfficeSelectList = (await officeService.GetStaffAsListItemsAsync(currentOfficeId)).ToSelectList();
    }
}
