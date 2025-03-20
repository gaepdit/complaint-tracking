using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Concerns;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.Staff;
using Cts.Domain.Data;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class EditModel(
    IComplaintService complaintService,
    IStaffService staffService,
    IConcernService concernService,
    IValidator<ComplaintUpdateDto> validator,
    IAuthorizationService authorization)
    : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public ComplaintUpdateDto Item { get; set; } =
        null!; // Caution: Object name is used in "copyContactInfo.js" script.

    public SelectList ConcernsSelectList { get; private set; } = null!;
    public SelectList StaffSelectList { get; private set; } = null!;
    public static SelectList StatesSelectList => new(Data.States);
    public static SelectList CountiesSelectList => new(Data.Counties);

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id <= 0) return RedirectToPage("Index");
        var item = await complaintService.FindForUpdateAsync(Id);
        if (item is null) return NotFound();
        if (!await UserCanEditAsync(item)) return Forbid();

        Item = item;
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var original = await complaintService.FindForUpdateAsync(Id);
        if (original is null || !await UserCanEditAsync(original)) return BadRequest();

        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await complaintService.UpdateAsync(Id, Item);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task PopulateSelectListsAsync()
    {
        ConcernsSelectList = (await concernService.GetAsListItemsAsync()).ToSelectList();
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
    }

    private Task<bool> UserCanEditAsync(ComplaintUpdateDto item) =>
        authorization.Succeeded(User, item, new ComplaintUpdateRequirement());
}
