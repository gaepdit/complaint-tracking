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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class AddModel(
    IComplaintService complaintService,
    IStaffService staffService,
    IConcernService concernService,
    IOfficeService officeService,
    IValidator<ComplaintCreateDto> validator)
    : PageModel
{
    [BindProperty]
    public ComplaintCreateDto NewComplaint { get; set; } = default!;

    public SelectList ConcernsSelectList { get; private set; } = default!;
    public SelectList OfficesSelectList { get; private set; } = default!;
    public SelectList AllActiveStaffSelectList { get; private set; } = default!;
    public SelectList ActiveStaffInOfficeSelectList { get; private set; } = default!;

    public SelectList StatesSelectList => new(Data.States);
    public SelectList CountiesSelectList => new(Data.Counties);

    public async Task OnGetAsync()
    {
        var user = await staffService.GetCurrentUserAsync();
        NewComplaint = new ComplaintCreateDto(user.Id, user.Office?.Id);
        await PopulateSelectListsAsync(user.Office?.Id);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await validator.ApplyValidationAsync(NewComplaint, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(NewComplaint.OfficeId);
            return Page();
        }

        var result = await complaintService.CreateAsync(NewComplaint, AppSettings.AttachmentServiceConfig);

        if (result.HasWarnings)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "Complaint successfully created. No files were attached because of the following errors: " +
                result.WarningsDisplay);
        }
        else
        {
            var successMessage = result.NumberOfAttachments switch
            {
                0 => "Complaint successfully created.",
                1 => "Complaint successfully created and one file attached.",
                _ => $"Complaint successfully created and {result.NumberOfAttachments} files attached.",
            };
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, successMessage);
        }

        return RedirectToPage("Details", new { id = result.ComplaintId });
    }

    private async Task PopulateSelectListsAsync(Guid? currentOfficeId)
    {
        ConcernsSelectList = (await concernService.GetAsListItemsAsync()).ToSelectList();
        OfficesSelectList = (await officeService.GetAsListItemsAsync()).ToSelectList();
        AllActiveStaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
        ActiveStaffInOfficeSelectList = (await officeService.GetStaffAsListItemsAsync(currentOfficeId)).ToSelectList();
    }
}
