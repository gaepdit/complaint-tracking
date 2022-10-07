using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.RazorHelpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Admin.Users;

[Authorize(Roles = AppRole.UserAdmin)]
public class EditModel : PageModel
{
    private readonly IStaffAppService _staffService;
    private readonly IOfficeAppService _officeService;
    private readonly IValidator<StaffUpdateDto> _validator;

    public EditModel(IStaffAppService staffService, IOfficeAppService officeService, IValidator<StaffUpdateDto> validator)
    {
        _staffService = staffService;
        _officeService = officeService;
        _validator = validator;
    }

    public StaffViewDto DisplayStaff { get; private set; } = default!;

    [BindProperty]
    public StaffUpdateDto UpdateStaff { get; set; } = default!;

    public SelectList OfficeItems { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();
        var staff = await _staffService.FindAsync(id.Value);
        if (staff == null) return NotFound("ID not found.");

        DisplayStaff = staff;
        UpdateStaff = DisplayStaff.AsUpdateDto();

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validationResult = await _validator.ValidateAsync(UpdateStaff);
        if (!validationResult.IsValid) validationResult.AddToModelState(ModelState, nameof(UpdateStaff));
        if (!ModelState.IsValid)
        {
            var staff = await _staffService.FindAsync(UpdateStaff.Id);
            if (staff == null) return BadRequest();

            DisplayStaff = staff;

            await PopulateSelectListsAsync();
            return Page();
        }

        await _staffService.UpdateAsync(UpdateStaff);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated.");
        return RedirectToPage("Details", new { id = UpdateStaff.Id });
    }

    private async Task PopulateSelectListsAsync() =>
        OfficeItems = (await _officeService.GetActiveListItemsAsync()).ToSelectList();
}
