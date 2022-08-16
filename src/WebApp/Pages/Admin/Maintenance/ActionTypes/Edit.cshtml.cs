using Cts.AppServices.ActionTypes;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.RazorHelpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;

public class Edit : PageModel
{
    private readonly IActionTypeAppService _service;
    private readonly IValidator<ActionTypeUpdateDto> _validator;

    public Edit(IActionTypeAppService service, IValidator<ActionTypeUpdateDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [BindProperty]
    public ActionTypeUpdateDto Item { get; set; } = default!;

    [BindProperty, HiddenInput]
    public string OriginalName { get; set; } = string.Empty;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.ActionType;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();
        var item = await _service.FindForUpdateAsync(id.Value);
        if (item == null) return NotFound("ID not found.");

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
