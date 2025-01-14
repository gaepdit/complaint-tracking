using Cts.AppServices.Concerns;
using Cts.AppServices.Permissions;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentValidation;

namespace Cts.WebApp.Pages.Admin.Maintenance.Concerns;

[Authorize(Policy = nameof(Policies.SiteMaintainer))]
public class AddModel : PageModel
{
    [BindProperty]
    public ConcernCreateDto Item { get; set; } = null!;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.Concern;

    public void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] IConcernService service,
        [FromServices] IValidator<ConcernCreateDto> validator)
    {
        await validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid) return Page();

        var id = await service.CreateAsync(Item.Name);

        HighlightId = id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully added.");
        return RedirectToPage("Index");
    }
}
