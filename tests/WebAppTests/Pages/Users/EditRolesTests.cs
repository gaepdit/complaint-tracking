using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.Domain.Identity;
using Cts.WebApp.Pages.Admin.Users;

namespace WebAppTests.Pages.Users;

public class EditRolesTests
{
    private static readonly OfficeViewDto OfficeViewTest = new(Guid.Empty, TextData.ValidName, true);

    private static readonly StaffViewDto StaffViewTest = new()
    {
        Id = Guid.Empty.ToString(),
        FamilyName = TextData.ValidName,
        GivenName = TextData.ValidName,
        Email = TextData.ValidEmail,
        Office = OfficeViewTest,
        Active = true,
    };

    private static readonly List<EditRolesModel.RoleSetting> RoleSettingsTest =
    [
        new EditRolesModel.RoleSetting
        {
            Name = TextData.ValidName,
            DisplayName = TextData.ValidName,
            Description = TextData.ValidName,
            IsSelected = true,
        },
    ];

    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        // Arrange
        var expectedRoleSettings = AppRole.AllRoles
            .Select(r => new EditRolesModel.RoleSetting
            {
                Name = r.Key,
                DisplayName = r.Value.DisplayName,
                Description = r.Value.Description,
                IsSelected = r.Key == RoleName.SiteMaintenance,
            }).ToList();

        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>())
            .Returns(StaffViewTest);
        staffServiceMock.GetRolesAsync(Arg.Any<string>())
            .Returns(new List<string> { RoleName.SiteMaintenance });

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var pageModel = new EditRolesModel(staffServiceMock, authorizationMock)
            { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await pageModel.OnGetAsync(StaffViewTest.Id);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        pageModel.DisplayStaff.Should().Be(StaffViewTest);
        pageModel.OfficeName.Should().Be(TextData.ValidName);
        pageModel.UserId.Should().Be(Guid.Empty.ToString());
        pageModel.RoleSettings.Should().BeEquivalentTo(expectedRoleSettings);
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        // Arrange
        var pageModel = new EditRolesModel(Substitute.For<IStaffService>(), Substitute.For<IAuthorizationService>())
            { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await pageModel.OnGetAsync(null);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>())
            .Returns((StaffViewDto?)null);

        var pageModel = new EditRolesModel(staffServiceMock, Substitute.For<IAuthorizationService>())
            { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await pageModel.OnGetAsync(Guid.Empty.ToString());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        // Arrange
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "User roles successfully updated.", []);

        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateRolesAsync(Arg.Any<string>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Success);
        staffServiceMock.GetRolesAsync(Arg.Any<string>())
            .Returns(new List<string> { RoleName.SiteMaintenance });

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var page = new EditRolesModel(staffServiceMock, authorizationMock)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsSetup.PageTempData(),
        };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        page.ModelState.IsValid.Should().BeTrue();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Details");
        ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(Guid.Empty.ToString());
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
    }

    [Test]
    public async Task OnPost_GivenMissingUser_ReturnsBadRequest()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateRolesAsync(Arg.Any<string>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Failed());
        staffServiceMock.FindAsync(Arg.Any<string>())
            .Returns((StaffViewDto?)null);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var page = new EditRolesModel(staffServiceMock, authorizationMock)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsSetup.PageTempData(),
            PageContext = WebAppTestsSetup.PageContextWithUser(),
        };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenUpdateFailure_ReturnsPageWithInvalidModelState()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateRolesAsync(Arg.Any<string>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Failed(new IdentityError { Code = "CODE", Description = "DESCRIPTION" }));
        staffServiceMock.FindAsync(Arg.Any<string>())
            .Returns(StaffViewTest);
        staffServiceMock.GetRolesAsync(Arg.Any<string>())
            .Returns(new List<string> { RoleName.SiteMaintenance });

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var page = new EditRolesModel(staffServiceMock, authorizationMock)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsSetup.PageTempData(),
            PageContext = WebAppTestsSetup.PageContextWithUser(),
        };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
        page.ModelState[string.Empty]!.Errors[0].ErrorMessage.Should().Be("CODE: DESCRIPTION");
        page.DisplayStaff.Should().Be(StaffViewTest);
        page.UserId.Should().Be(Guid.Empty.ToString());
        page.RoleSettings.Should().BeEquivalentTo(RoleSettingsTest);
    }
}
