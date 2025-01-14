using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace Cts.WebApp.Pages.Account;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class EditModel(IStaffService staffService, IOfficeService officeService, IValidator<StaffUpdateDto> validator)
    : PageModel
{
    [BindProperty]
    public StaffUpdateDto UpdateStaff { get; set; } = null!;

    public StaffViewDto DisplayStaff { get; private set; } = null!;

    public SelectList OfficeSelectList { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        DisplayStaff = await staffService.GetCurrentUserAsync();
        UpdateStaff = DisplayStaff.AsUpdateDto();

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var staff = await staffService.GetCurrentUserAsync();

        // User cannot deactivate self.
        UpdateStaff.Active = true;

        await validator.ApplyValidationAsync(UpdateStaff, ModelState);

        if (!ModelState.IsValid)
        {
            DisplayStaff = staff;
            await PopulateSelectListsAsync();
            return Page();
        }

        var result = await staffService.UpdateAsync(staff.Id, UpdateStaff);
        if (!result.Succeeded) return BadRequest();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated profile.");
        return RedirectToPage("Index");
    }

    private async Task PopulateSelectListsAsync() =>
        OfficeSelectList = (await officeService.GetAsListItemsAsync()).ToSelectList();
}
