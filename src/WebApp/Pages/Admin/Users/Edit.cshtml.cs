using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.UserAdministrator))]
public class EditModel : PageModel
{
    // Constructor
    private readonly IStaffService _staffService;
    private readonly IOfficeService _officeService;
    private readonly IValidator<StaffUpdateDto> _validator;

    public EditModel(
        IStaffService staffService,
        IOfficeService officeService,
        IValidator<StaffUpdateDto> validator)
    {
        _staffService = staffService;
        _officeService = officeService;
        _validator = validator;
    }

    // Properties
    [BindProperty]
    public StaffUpdateDto Item { get; set; } = default!;

    public StaffViewDto DisplayStaff { get; private set; } = default!;

    // Select lists
    public SelectList OfficesSelectList { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null) return RedirectToPage("Index");
        var staff = await _staffService.FindAsync(id);
        if (staff is null) return NotFound();

        DisplayStaff = staff;
        Item = DisplayStaff.AsUpdateDto();

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            var staff = await _staffService.FindAsync(Item.Id);
            if (staff is null) return BadRequest();

            DisplayStaff = staff;

            await PopulateSelectListsAsync();
            return Page();
        }

        var result = await _staffService.UpdateAsync(Item);
        if (!result.Succeeded) return BadRequest();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated.");
        return RedirectToPage("Details", new { id = Item.Id });
    }

    private async Task PopulateSelectListsAsync() =>
        OfficesSelectList = (await _officeService.GetActiveListItemsAsync()).ToSelectList();
}
