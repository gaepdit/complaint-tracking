using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using Cts.WebApp.Pages.Admin.Users;

namespace WebAppTests.Pages.Users;

public class DetailsTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var staffView = new StaffViewDto
        {
            Id = Guid.Empty.ToString(),
            FamilyName = TextData.ValidName,
            GivenName = TextData.ValidName,
            Email = TextData.ValidEmail,
            Active = true,
        };

        var serviceMock = Substitute.For<IStaffService>();
        serviceMock.FindAsync(Arg.Any<string>()).Returns(staffView);
        serviceMock.GetAppRolesAsync(Arg.Any<string>()).Returns(new List<AppRole>());
        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());
        var pageModel = new DetailsModel { TempData = WebAppTestsSetup.PageTempData() };

        var result = await pageModel.OnGetAsync(serviceMock, authorizationMock, staffView.Id);

        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        pageModel.DisplayStaff.Should().Be(staffView);
        pageModel.Roles.Should().BeEmpty();
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var serviceMock = Substitute.For<IStaffService>();
        var pageModel = new DetailsModel { TempData = WebAppTestsSetup.PageTempData() };

        var result = await pageModel.OnGetAsync(serviceMock, Substitute.For<IAuthorizationService>(), null);

        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var serviceMock = Substitute.For<IStaffService>();
        serviceMock.FindAsync(Arg.Any<string>()).Returns((StaffViewDto?)null);
        var pageModel = new DetailsModel { TempData = WebAppTestsSetup.PageTempData() };

        var result =
            await pageModel.OnGetAsync(serviceMock, Substitute.For<IAuthorizationService>(), Guid.Empty.ToString());

        result.Should().BeOfType<NotFoundResult>();
    }
}
