using Cts.AppServices.Concerns;
using Cts.AppServices.Permissions;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cts.WebApp.Pages.Admin.Maintenance.Concerns;

[Authorize(Policy = nameof(Policies.SiteMaintainer))]
public class EditModel : PageModel
{
    private readonly IConcernService _service;
    private readonly IValidator<ConcernUpdateDto> _validator;

    public EditModel(
        IConcernService service,
        IValidator<ConcernUpdateDto> validator)
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
        if (id is null) return RedirectToPage("Index");
        var item = await _service.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();

        Item = item;
        OriginalName = Item.Name;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid) return Page();

        await _service.UpdateAsync(Item);

        HighlightId = Item.Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully updated.");
        return RedirectToPage("Index");
    }
}
