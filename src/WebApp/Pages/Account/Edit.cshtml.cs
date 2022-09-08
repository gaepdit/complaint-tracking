using Cts.AppServices.Offices;
using Cts.AppServices.StaffServices;
using Cts.AppServices.UserServices;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.RazorHelpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using GaEpd.Library.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Account;

[Authorize]
public class EditModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IStaffAppService _staffService;
    private readonly IOfficeAppService _officeService;
    private readonly IValidator<StaffUpdateDto> _validator;

    public EditModel(
        IUserService userService,
        IStaffAppService staffService,
        IOfficeAppService officeService,
        IValidator<StaffUpdateDto> validator)
    {
        _userService = userService;
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
        var currentUser = await _userService.GetCurrentUserAsync();
        if (currentUser is null) return Forbid();

        var staff = await _staffService.FindAsync(currentUser.IdAsGuid);
        if (staff is not { Active: true }) return Forbid();

        DisplayStaff = staff;
        UpdateStaff = DisplayStaff.AsUpdateDto();

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var currentUser = await _userService.GetCurrentUserAsync();
        if (currentUser is null) return Forbid();
        if (currentUser.IdAsGuid != UpdateStaff.Id || !UpdateStaff.Active) return BadRequest();

        var validationResult = await _validator.ValidateAsync(UpdateStaff);
        if (!validationResult.IsValid) validationResult.AddToModelState(ModelState, nameof(UpdateStaff));
        
        if (!ModelState.IsValid)
        {
            var staff = await _staffService.FindAsync(UpdateStaff.Id);
            if (staff is not { Active: true }) return Forbid();

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
