using Cts.AppServices.ActionTypes;
using Cts.AppServices.Permissions;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentValidation;

namespace Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;

[Authorize(Policy = nameof(Policies.SiteMaintainer))]
public class EditModel(IActionTypeService service, IValidator<ActionTypeUpdateDto> validator) : PageModel
{
    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public ActionTypeUpdateDto Item { get; set; } = null!;

    [BindProperty]
    public string OriginalName { get; set; } = string.Empty;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.ActionType;

    public async Task<IActionResult> OnGetAsync()
    {
        var item = await service.FindForUpdateAsync(Id);
        if (item is null) return NotFound();

        Item = item;
        OriginalName = Item.Name;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await validator.ApplyValidationAsync(Item, ModelState, Id);
        if (!ModelState.IsValid) return Page();

        await service.UpdateAsync(Id, Item);

        HighlightId = Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully updated.");
        return RedirectToPage("Index");
    }
}
