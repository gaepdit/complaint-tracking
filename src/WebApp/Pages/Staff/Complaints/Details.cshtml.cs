using Cts.AppServices.ActionTypes;
using Cts.AppServices.Attachments;
using Cts.AppServices.Attachments.Dto;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.ComplaintActions.Dto;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Permissions;
using Cts.AppServices.Permissions.Helpers;
using Cts.AppServices.Staff;
using Cts.Domain.Data;
using Cts.WebApp.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using Cts.WebApp.Platform.Settings;
using GaEpd.AppLibrary.ListItems;

namespace Cts.WebApp.Pages.Staff.Complaints;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DetailsModel(
    IComplaintService complaintService,
    IActionService actionService,
    IActionTypeService actionTypeService,
    IAttachmentService attachmentService,
    IStaffService staffService,
    IAuthorizationService authorization)
    : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public ComplaintViewDto ComplaintView { get; private set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; private set; } = new();

    public ActionCreateDto NewAction { get; set; } = null!;

    public AttachmentsUploadDto FileUploads { get; set; } = null!;

    [TempData]
    public Guid HighlightId { get; set; }

    public string? ValidatingSection { get; private set; }
    public SelectList ActionItemTypeSelectList { get; private set; } = null!;
    public bool ShowHistoryNotification => ComplaintView.EarliestTransition < DomainConstants.OracleMigrationDate;

    public bool ViewableActions => ComplaintView.Actions.Exists(action =>
        !action.IsDeleted || UserCan[ComplaintOperation.ViewDeletedActions]);

    [TempData]
    public bool UploadSuccess { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id <= 0) return RedirectToPage("Index");

        var complaintView = await complaintService.FindAsync(Id, true);
        if (complaintView is null) return NotFound();

        await SetPermissionsAsync(complaintView);
        if (complaintView.IsDeleted && !UserCan[ComplaintOperation.ManageDeletions]) return NotFound();

        ComplaintView = complaintView;
        var investigator = (await staffService.GetCurrentUserAsync()).Name;
        NewAction = new ActionCreateDto(Id) { Investigator = investigator };
        await PopulateSelectListsAsync();

        return Page();
    }

    /// OnPostAccept is used for the current user to accept the Complaint.
    public async Task<IActionResult> OnPostAcceptAsync(CancellationToken token)
    {
        if (Id <= 0) return BadRequest();

        var complaintView = await complaintService.FindAsync(Id, includeDeletedActions: true, token: token);
        if (complaintView is null || complaintView.IsDeleted) return BadRequest();

        await SetPermissionsAsync(complaintView);
        if (!UserCan[ComplaintOperation.Accept]) return BadRequest();

        await complaintService.AcceptAsync(Id, token: token);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Complaint accepted.");
        return RedirectToPage("Details", routeValues: new { Id });
    }

    /// PostNewAction is used to add a new Action for this Complaint.
    public async Task<IActionResult> OnPostNewActionAsync(ActionCreateDto newAction, CancellationToken token)
    {
        if (Id <= 0 || newAction.ComplaintId != Id) return BadRequest();

        var complaintView = await complaintService.FindAsync(Id, includeDeletedActions: true, token: token);
        if (complaintView is null || complaintView.IsDeleted) return BadRequest();

        await SetPermissionsAsync(complaintView);
        if (!UserCan[ComplaintOperation.EditActions]) return BadRequest();

        if (!ModelState.IsValid)
        {
            ValidatingSection = nameof(OnPostNewActionAsync);
            ComplaintView = complaintView;
            await PopulateSelectListsAsync();
            return Page();
        }

        HighlightId = await actionService.CreateAsync(newAction, token: token);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "New Action successfully added.");
        return RedirectToPage("Details", pageHandler: null, routeValues: new { Id }, fragment: HighlightId.ToString());
    }

    /// PostUploadFiles is used to add attachment files to this Complaint.
    public async Task<IActionResult> OnPostUploadFilesAsync(AttachmentsUploadDto fileUploads, CancellationToken token)
    {
        if (Id <= 0) return BadRequest();

        var complaintView = await complaintService.FindAsync(Id, includeDeletedActions: true, token: token);
        if (complaintView is null || complaintView.IsDeleted) return BadRequest();

        await SetPermissionsAsync(complaintView);
        if (!UserCan[ComplaintOperation.EditAttachments]) return BadRequest();

        if (!ModelState.IsValid)
        {
            ValidatingSection = nameof(OnPostUploadFilesAsync);
            ComplaintView = complaintView;
            var investigator = (await staffService.GetCurrentUserAsync()).Name;
            NewAction = new ActionCreateDto(Id) { Investigator = investigator };
            await PopulateSelectListsAsync();
            return Page();
        }

        await attachmentService.SaveAttachmentsAsync(Id, fileUploads.Files, AppSettings.AttachmentServiceConfig,
            token);
        UploadSuccess = true;
        return RedirectToPage("Details", new { Id });
    }

    private async Task PopulateSelectListsAsync() =>
        ActionItemTypeSelectList = (await actionTypeService.GetAsListItemsAsync()).ToSelectList();

    private async Task SetPermissionsAsync(ComplaintViewDto item)
    {
        foreach (var operation in ComplaintOperation.AllOperations)
            UserCan[operation] = await authorization.Succeeded(User, item, operation);
    }
}
