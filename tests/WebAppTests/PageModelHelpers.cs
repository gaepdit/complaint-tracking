using Cts.AppServices.ActionTypes;
using Cts.AppServices.Attachments;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.WebApp.Pages.Staff.Complaints;

namespace WebAppTests;

internal static class PageModelHelpers
{
    private static readonly StaffViewDto StaffViewTest = new() { Id = Guid.Empty.ToString(), Active = true };

    public static DetailsModel BuildDetailsPageModel(
        IComplaintService? complaintService = null,
        IActionService? actionService = null,
        IActionTypeService? actionTypeService = null,
        IAttachmentService? attachmentService = null,
        IAuthorizationService? authorizationService = null)
    {
        var staffService = Substitute.For<IStaffService>();
        staffService.GetCurrentUserAsync().Returns(StaffViewTest);

        return new DetailsModel(complaintService ?? Substitute.For<IComplaintService>(),
            actionService ?? Substitute.For<IActionService>(),
            actionTypeService ?? Substitute.For<IActionTypeService>(),
            attachmentService ?? Substitute.For<IAttachmentService>(),
            staffService,
            authorizationService ?? Substitute.For<IAuthorizationService>());
    }
}
