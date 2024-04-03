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
        // A
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

        // A
        var result = await pageModel.OnGetAsync(StaffViewTest.Id);

        // A
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
        // A
        var pageModel = new EditRolesModel(Substitute.For<IStaffService>(), Substitute.For<IAuthorizationService>())
            { TempData = WebAppTestsSetup.PageTempData() };

        // A
        var result = await pageModel.OnGetAsync(null);

        // A
        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        // A
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>())
            .Returns((StaffViewDto?)null);

        var pageModel = new EditRolesModel(staffServiceMock, Substitute.For<IAuthorizationService>())
            { TempData = WebAppTestsSetup.PageTempData() };

        // A
        var result = await pageModel.OnGetAsync(Guid.Empty.ToString());

        // A
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        // A
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

        // A
        var result = await page.OnPostAsync();

        // A
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
        // A
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

        // A
        var result = await page.OnPostAsync();

        // A
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenUpdateFailure_ReturnsPageWithInvalidModelState()
    {
        // A
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

        // A
        var result = await page.OnPostAsync();

        // A
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
        page.ModelState[string.Empty]!.Errors[0].ErrorMessage.Should().Be("CODE: DESCRIPTION");
        page.DisplayStaff.Should().Be(StaffViewTest);
        page.UserId.Should().Be(Guid.Empty.ToString());
        page.RoleSettings.Should().BeEquivalentTo(RoleSettingsTest);
    }
}
