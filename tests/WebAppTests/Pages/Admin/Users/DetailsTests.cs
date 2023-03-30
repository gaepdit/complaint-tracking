using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Admin.Users;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace WebAppTests.Pages.Admin.Users;

public class DetailsTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var staffView = new StaffViewDto
        {
            Id = Guid.Empty.ToString(),
            Email = TestConstants.ValidEmail,
            GivenName = TestConstants.ValidName,
            FamilyName = TestConstants.ValidName,
        };
        var serviceMock = new Mock<IStaffAppService>();
        serviceMock.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync(staffView);
        serviceMock.Setup(l => l.GetAppRolesAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<AppRole>());
        var authorizationMock = new Mock<IAuthorizationService>();
        authorizationMock.Setup(l => l.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), null, It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success);
        var pageModel = new DetailsModel { TempData = WebAppTestsGlobal.PageTempData() };

        var result = await pageModel.OnGetAsync(serviceMock.Object, authorizationMock.Object, staffView.Id);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(staffView);
            pageModel.Roles.Should().BeEmpty();
            pageModel.Message.Should().BeNull();
        }
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var serviceMock = new Mock<IStaffAppService>();
        var pageModel = new DetailsModel { TempData = WebAppTestsGlobal.PageTempData() };

        var result = await pageModel.OnGetAsync(serviceMock.Object, Mock.Of<IAuthorizationService>(), null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var serviceMock = new Mock<IStaffAppService>();
        serviceMock.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync((StaffViewDto?)null);
        var pageModel = new DetailsModel { TempData = WebAppTestsGlobal.PageTempData() };

        var result = await pageModel
            .OnGetAsync(serviceMock.Object, Mock.Of<IAuthorizationService>(), Guid.Empty.ToString());

        result.Should().BeOfType<NotFoundResult>();
    }
}
