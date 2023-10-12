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
public class EditModel : PageModel
{
    // Constructor
    private readonly IComplaintService _complaintService;
    private readonly IStaffService _staffService;
    private readonly IConcernService _concernService;
    private readonly IValidator<ComplaintUpdateDto> _validator;
    private readonly IAuthorizationService _authorization;

    public EditModel(IComplaintService complaintService, IStaffService staffService, IConcernService concernService,
        IValidator<ComplaintUpdateDto> validator, IAuthorizationService authorization)
    {
        _complaintService = complaintService;
        _staffService = staffService;
        _concernService = concernService;
        _validator = validator;
        _authorization = authorization;
    }

    // Properties

    [FromRoute]
    public int Id { get; set; }

    [BindProperty]
    public ComplaintUpdateDto Item { get; set; } = default!;

    // Select lists
    public SelectList ConcernsSelectList { get; private set; } = default!;
    public SelectList AllStaffSelectList { get; private set; } = default!;
    public SelectList StatesSelectList => new(Data.States);
    public SelectList CountiesSelectList => new(Data.Counties);

    // Methods
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await _complaintService.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();
        if (!await UserCanEditAsync(item)) return Forbid();

        Id = id.Value;
        Item = item;
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var original = await _complaintService.FindForUpdateAsync(Id);
        if (original is null) return BadRequest();
        if (!await UserCanEditAsync(original)) return BadRequest();

        await _validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await _complaintService.UpdateAsync(Id, Item);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task PopulateSelectListsAsync()
    {
        ConcernsSelectList = (await _concernService.GetActiveListItemsAsync()).ToSelectList();
        AllStaffSelectList = (await _staffService.GetStaffListItemsAsync(true)).ToSelectList();
    }

    private async Task<bool> UserCanEditAsync(ComplaintUpdateDto item) =>
        (await _authorization.AuthorizeAsync(User, item, ComplaintOperation.EditDetails)).Succeeded;
}
