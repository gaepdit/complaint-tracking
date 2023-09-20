using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize(Policy = nameof(Policies.SiteMaintainer))]
public class EditModel : PageModel
{
    // Constructor
    private readonly IOfficeService _officeService;
    private readonly IStaffService _staffService;
    private readonly IValidator<OfficeUpdateDto> _validator;

    public EditModel(
        IOfficeService officeService,
        IStaffService staffService,
        IValidator<OfficeUpdateDto> validator)
    {
        _officeService = officeService;
        _staffService = staffService;
        _validator = validator;
    }

    // Properties
    [BindProperty]
    public OfficeUpdateDto Item { get; set; } = default!;

    [BindProperty]
    public string OriginalName { get; set; } = string.Empty;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.Office;

    // Select lists
    public SelectList ActiveStaffMembers { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await _officeService.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();

        Item = item;
        OriginalName = Item.Name;

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await _officeService.UpdateAsync(Item);

        HighlightId = Item.Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully updated.");
        return RedirectToPage("Index");
    }

    private async Task PopulateSelectListsAsync() =>
        ActiveStaffMembers = (await _staffService.GetStaffListItemsAsync(true)).ToSelectList();
}
