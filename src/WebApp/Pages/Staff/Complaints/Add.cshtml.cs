using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.CommandDto;
using Cts.AppServices.Concerns;
using Cts.AppServices.Offices;
using Cts.AppServices.Permissions;
using Cts.AppServices.Staff;
using Cts.Domain.Data;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Cts.WebApp.Platform.Settings;
using FluentValidation;
using GaEpd.AppLibrary.ListItems;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class AddModel(
    IComplaintService complaintService,
    IStaffService staffService,
    IConcernService concernService,
    IOfficeService officeService,
    IValidator<ComplaintCreateDto> validator) : PageModel
{
    [BindProperty]
    public ComplaintCreateDto Item { get; set; } =
        null!; // Caution: Object name is used in "copyContactInfo.js" script.

    public SelectList ConcernsSelectList { get; private set; } = null!;
    public SelectList OfficesSelectList { get; private set; } = null!;
    public SelectList AllActiveStaffSelectList { get; private set; } = null!;
    public SelectList ActiveStaffInOfficeSelectList { get; private set; } = null!;

    public static SelectList StatesSelectList => new(Data.States);
    public static SelectList CountiesSelectList => new(Data.Counties);

    public async Task OnGetAsync()
    {
        var user = await staffService.GetCurrentUserAsync();
        Item = new ComplaintCreateDto(user.Id, user.Office?.Id);
        await PopulateSelectListsAsync(user.Office?.Id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(Item.OfficeId);
            return Page();
        }

        var createResult =
            await complaintService.CreateAsync(Item, AppSettings.AttachmentServiceConfig, this.GetBaseUrl());

        var message = createResult.NumberOfAttachments switch
        {
            0 => "Complaint successfully created.",
            1 => "Complaint successfully created and one file attached.",
            _ => $"Complaint successfully created and {createResult.NumberOfAttachments} files attached.",
        };

        TempData.SetDisplayMessage(
            createResult.HasWarnings ? DisplayMessage.AlertContext.Warning : DisplayMessage.AlertContext.Success,
            message, createResult.Warnings);

        return RedirectToPage("Details", new { id = createResult.ComplaintId });
    }

    private async Task PopulateSelectListsAsync(Guid? currentOfficeId)
    {
        ConcernsSelectList = (await concernService.GetAsListItemsAsync()).ToSelectList();
        OfficesSelectList = (await officeService.GetAsListItemsAsync()).ToSelectList();
        AllActiveStaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
        ActiveStaffInOfficeSelectList = (await officeService.GetStaffAsListItemsAsync(currentOfficeId)).ToSelectList();
    }
}
