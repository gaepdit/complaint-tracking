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
public class AddModel : PageModel
{
    // Constructor
    private readonly IComplaintService _complaints;
    private readonly IStaffService _staff;
    private readonly IConcernService _concerns;
    private readonly IOfficeService _offices;
    private readonly IValidator<ComplaintCreateDto> _validator;

    public AddModel(
        IComplaintService service,
        IStaffService staff,
        IConcernService concerns,
        IOfficeService offices,
        IValidator<ComplaintCreateDto> validator)
    {
        _complaints = service;
        _staff = staff;
        _concerns = concerns;
        _offices = offices;
        _validator = validator;
    }

    // Properties
    [BindProperty]
    public ComplaintCreateDto Item { get; set; } = default!;

    // Select lists
    public SelectList ConcernsSelectList { get; private set; } = default!;
    public SelectList OfficesSelectList { get; private set; } = default!;
    public SelectList AllStaffSelectList { get; private set; } = default!;
    public SelectList StaffInOfficeSelectList { get; private set; } = default!;

    public SelectList StatesSelectList => new(Data.States);
    public SelectList CountiesSelectList => new(Data.Counties);

    // Methods
    public async Task OnGetAsync()
    {
        var user = await _staff.GetCurrentUserAsync();
        Item = new ComplaintCreateDto(user.Id, user.Office?.Id);
        await PopulateSelectListsAsync(user.Office?.Id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(Item.CurrentOfficeId);
            return Page();
        }

        var id = await _complaints.CreateAsync(Item);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint successfully created.");
        return RedirectToPage("Details", new { id });
    }

    private async Task PopulateSelectListsAsync(Guid? currentOfficeId)
    {
        ConcernsSelectList = (await _concerns.GetActiveListItemsAsync()).ToSelectList();
        OfficesSelectList = (await _offices.GetActiveListItemsAsync()).ToSelectList();
        AllStaffSelectList = (await _staff.GetStaffListItemsAsync(true)).ToSelectList();
        StaffInOfficeSelectList = (await _offices.GetStaffListItemsAsync(currentOfficeId, true)).ToSelectList();
    }
}
