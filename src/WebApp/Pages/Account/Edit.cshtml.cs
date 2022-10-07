using FluentValidation;
using FluentValidation.AspNetCore;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyAppRoot.AppServices.Offices;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.WebApp.Models;
using MyAppRoot.WebApp.Platform.RazorHelpers;

namespace MyAppRoot.WebApp.Pages.Account;

[Authorize]
public class EditModel : PageModel
{
    private readonly IStaffAppService _staffService;
    private readonly IOfficeAppService _officeService;
    private readonly IValidator<StaffUpdateDto> _validator;

    public EditModel(
        IStaffAppService staffService,
        IOfficeAppService officeService,
        IValidator<StaffUpdateDto> validator)
    {
        _staffService = staffService;
        _officeService = officeService;
        _validator = validator;
    }

    public StaffViewDto DisplayStaff { get; private set; } = default!;

    [BindProperty]
    public StaffUpdateDto UpdateStaff { get; set; } = default!;

    public SelectList OfficeItems { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var staff = await _staffService.GetCurrentUserAsync();
        if (staff is not { Active: true }) return Forbid();

        DisplayStaff = staff;
        UpdateStaff = DisplayStaff.AsUpdateDto();

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var staff = await _staffService.GetCurrentUserAsync();
        if (staff is not { Active: true }) return Forbid();
        if (staff.Id != UpdateStaff.Id || !UpdateStaff.Active) return BadRequest();

        var validationResult = await _validator.ValidateAsync(UpdateStaff);
        if (!validationResult.IsValid) validationResult.AddToModelState(ModelState, nameof(UpdateStaff));

        if (!ModelState.IsValid)
        {
            DisplayStaff = staff;
            await PopulateSelectListsAsync();
            return Page();
        }

        await _staffService.UpdateAsync(UpdateStaff);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated profile.");
        return RedirectToPage("Index");
    }

    private async Task PopulateSelectListsAsync() =>
        OfficeItems = (await _officeService.GetActiveListItemsAsync()).ToSelectList();
}
