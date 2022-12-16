using Cts.AppServices.Concerns;
using Cts.Domain.Identity;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.RazorHelpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Maintenance.Concerns;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class EditModel : PageModel
{
    private readonly IConcernAppService _service;
    private readonly IValidator<ConcernUpdateDto> _validator;

    public EditModel(IConcernAppService service, IValidator<ConcernUpdateDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [BindProperty]
    public ConcernUpdateDto Item { get; set; } = default!;

    [BindProperty]
    public string OriginalName { get; set; } = string.Empty;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.Concern;

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
