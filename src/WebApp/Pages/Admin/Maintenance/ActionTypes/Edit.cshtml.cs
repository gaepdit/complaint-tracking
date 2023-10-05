using Cts.AppServices.ActionTypes;
using Cts.AppServices.Permissions;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;

[Authorize(Policy = nameof(Policies.SiteMaintainer))]
public class EditModel : PageModel
{
    // Constructor
    private readonly IActionTypeService _service;
    private readonly IValidator<ActionTypeUpdateDto> _validator;

    public EditModel(IActionTypeService service, IValidator<ActionTypeUpdateDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    // Properties
    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public ActionTypeUpdateDto Item { get; set; } = default!;

    [BindProperty]
    public string OriginalName { get; set; } = string.Empty;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.ActionType;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await _service.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();

        Item = item;
        OriginalName = Item.Name;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _validator.ApplyValidationAsync(Item, ModelState, Id);
        if (!ModelState.IsValid) return Page();

        await _service.UpdateAsync(Id, Item);

        HighlightId = Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully updated.");
        return RedirectToPage("Index");
    }
}
