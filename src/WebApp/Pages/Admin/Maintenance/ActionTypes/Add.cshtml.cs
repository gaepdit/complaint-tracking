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
public class AddModel : PageModel
{
    [BindProperty]
    public ActionTypeCreateDto Item { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.ActionType;

    public void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] IActionTypeAppService service,
        [FromServices] IValidator<ActionTypeCreateDto> validator)
    {
        var validationResult = await validator.ValidateAsync(Item);
        if (!validationResult.IsValid) validationResult.AddToModelState(ModelState, nameof(Item));
        if (!ModelState.IsValid) return Page();

        var id = await service.CreateAsync(Item.Name);

        HighlightId = id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"\"{Item.Name}\" successfully added.");
        return RedirectToPage("Index");
    }
}
