using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.WebApp.Pages.Staff.Complaints;
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
        var serviceMock = Substitute.For<IComplaintService>();
        serviceMock.FindAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(ItemTest);
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetCurrentUserAsync()
            .Returns(StaffViewTest);
        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Is<IAuthorizationRequirement[]>(x => x.Contains(ComplaintOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Success());
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Is<IAuthorizationRequirement[]>(x => !x.Contains(ComplaintOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Failed());
        var page = new DetailsModel(serviceMock, staffServiceMock, authorizationMock)
            { TempData = WebAppTestsSetup.PageTempData(), PageContext = WebAppTestsSetup.PageContextWithUser() };

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
