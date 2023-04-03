using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Staff;
using Cts.WebApp.Pages.Admin.Complaints;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebAppTests.Pages.Admin.Complaints;

public class DetailsTests
{
    private static readonly ComplaintViewDto ItemTest = new() { Id = 0 };

    private static readonly StaffViewDto StaffViewTest = new() { Id = Guid.Empty.ToString(), Active = true };


    [Test]
    public async Task OnGet_GivenManageDeletions_ReturnsWithItemAndPermissions()
    {
        var serviceMock = new Mock<IComplaintAppService>();
        serviceMock.Setup(l => l.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ItemTest);
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync(StaffViewTest);
        var authorizationMock = new Mock<IAuthorizationService>();
        authorizationMock.Setup(l =>
                l.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object?>(),
                    It.IsAny<IAuthorizationRequirement[]>()))
            .ReturnsAsync(AuthorizationResult.Failed);
        authorizationMock.Setup(l =>
                l.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object?>(),
                    new[] { ComplaintOperation.ManageDeletions }))
            .ReturnsAsync(AuthorizationResult.Success);
        var page = new DetailsModel(serviceMock.Object, staffServiceMock.Object, authorizationMock.Object)
            { TempData = WebAppTestsGlobal.PageTempData(), PageContext = WebAppTestsGlobal.PageContextWithUser() };

        await page.OnGetAsync(ItemTest.Id);

        using (new AssertionScope())
        {
            page.Item.Should().BeEquivalentTo(ItemTest);
            page.UserCan.Should().NotBeEmpty();
            page.UserCan[ComplaintOperation.ManageDeletions].Should().BeTrue();
            page.UserCan[ComplaintOperation.Accept].Should().BeFalse();
        }
    }
}
