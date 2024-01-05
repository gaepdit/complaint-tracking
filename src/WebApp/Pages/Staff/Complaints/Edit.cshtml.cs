using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Concerns;
using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using Cts.Domain.Data;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    public ComplaintUpdateDto Item { get; set; } = default!;

    public SelectList ConcernsSelectList { get; private set; } = default!;
    public SelectList AllActiveStaffSelectList { get; private set; } = default!;
    public SelectList StatesSelectList => new(Data.States);
    public SelectList CountiesSelectList => new(Data.Counties);

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await complaintService.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();
        if (!await UserCanEditAsync(item)) return Forbid();

        Id = id.Value;
        Item = item;
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var original = await complaintService.FindForUpdateAsync(Id);
        if (original is null) return BadRequest();
        if (!await UserCanEditAsync(original)) return BadRequest();

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
        ConcernsSelectList = (await concernService.GetActiveListItemsAsync()).ToSelectList();
        AllActiveStaffSelectList = (await staffService.GetStaffListItemsAsync()).ToSelectList();
    }

    private async Task<bool> UserCanEditAsync(ComplaintUpdateDto item) =>
        (await authorization.AuthorizeAsync(User, item, ComplaintOperation.EditDetails)).Succeeded;
}
