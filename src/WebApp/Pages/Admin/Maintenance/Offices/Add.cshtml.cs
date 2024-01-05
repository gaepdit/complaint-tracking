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
public class AddModel(IOfficeService officeService, IStaffService staffService, IValidator<OfficeCreateDto> validator)
    : PageModel
{
    [BindProperty]
    public OfficeCreateDto Item { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.Office;

    public SelectList ActiveStaffMembersSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        HighlightId = await officeService.CreateAsync(Item);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully added.");
        return RedirectToPage("Index");
    }

    private async Task PopulateSelectListsAsync() =>
        ActiveStaffMembersSelectList = (await staffService.GetStaffListItemsAsync()).ToSelectList();
}
