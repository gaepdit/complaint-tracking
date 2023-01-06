using Cts.AppServices.ActionTypes;
using Cts.Domain.Identity;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageDisplayHelpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class EditModel : PageModel
{
    private readonly IActionTypeAppService _service;
    private readonly IValidator<ActionTypeUpdateDto> _validator;

    public EditModel(IActionTypeAppService service, IValidator<ActionTypeUpdateDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [BindProperty]
    public ActionTypeUpdateDto Item { get; set; } = default!;

    [BindProperty]
    public string OriginalName { get; set; } = string.Empty;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.ActionType;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return RedirectToPage("Index");
        var item = await _service.FindForUpdateAsync(id.Value);
        if (item == null) return NotFound();

        Item = item;
        OriginalName = Item.Name;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validationResult = await _validator.ValidateAsync(Item);
        if (!validationResult.IsValid) validationResult.AddToModelState(ModelState, nameof(Item));
        if (!ModelState.IsValid) return Page();

        await _service.UpdateAsync(Item);

        HighlightId = Item.Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"\"{Item.Name}\" successfully updated.");
        return RedirectToPage("Index");
    }
}
