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
public class EditModel(IOfficeService officeService, IStaffService staffService, IValidator<OfficeUpdateDto> validator)
    : PageModel
{
    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public OfficeUpdateDto Item { get; set; } = default!;

    [BindProperty]
    public string OriginalName { get; set; } = string.Empty;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.Office;

    public SelectList ActiveStaffMembersSelectList { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await officeService.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();

        Item = item;
        OriginalName = Item.Name;

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await validator.ApplyValidationAsync(Item, ModelState, Id);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await officeService.UpdateAsync(Id, Item);

        HighlightId = Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully updated.");
        return RedirectToPage("Index");
    }

    private async Task PopulateSelectListsAsync() =>
        ActiveStaffMembersSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
