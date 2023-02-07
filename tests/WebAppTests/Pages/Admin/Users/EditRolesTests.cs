using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Admin.Users;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageDisplayHelpers;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Admin.Users;

public class EditRolesTests
{
    private static readonly OfficeViewDto OfficeViewTest = new() { Id = Guid.Empty, Name = TestConstants.ValidName };

    private static readonly StaffViewDto StaffViewTest = new()
    {
        Id = Guid.Empty.ToString(),
        Email = TestConstants.ValidEmail,
        GivenName = TestConstants.ValidName,
        FamilyName = TestConstants.ValidName,
        Office = OfficeViewTest,
    };

    private static readonly List<EditRolesModel.RoleSetting> RoleSettingsTest = new()
    {
        new EditRolesModel.RoleSetting
        {
            Name = TestConstants.ValidName,
            DisplayName = TestConstants.ValidName,
            Description = TestConstants.ValidName,
            IsSelected = true,
        },
    };

    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var expectedRoleSettings = AppRole.AllRoles
            .Select(r => new EditRolesModel.RoleSetting
            {
                Name = r.Key,
                DisplayName = r.Value.DisplayName,
                Description = r.Value.Description,
                IsSelected = r.Key == AppRole.SiteMaintenance,
            }).ToList();

        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync(StaffViewTest);
        staffServiceMock.Setup(l => l.GetRolesAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<string> { AppRole.SiteMaintenance });
        var pageModel = new EditRolesModel(staffServiceMock.Object) { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(StaffViewTest.Id);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(StaffViewTest);
            pageModel.OfficeName.Should().Be(TestConstants.ValidName);
            pageModel.UserId.Should().Be(Guid.Empty.ToString());
            pageModel.RoleSettings.Should().BeEquivalentTo(expectedRoleSettings);
        }
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var staffServiceMock = new Mock<IStaffAppService>();
        var pageModel = new EditRolesModel(staffServiceMock.Object) { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync((StaffViewDto?)null);
        var pageModel = new EditRolesModel(staffServiceMock.Object) { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(Guid.Empty.ToString());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "User roles successfully updated.");

        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.UpdateRolesAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, bool>>()))
            .ReturnsAsync(IdentityResult.Success);
        staffServiceMock.Setup(l => l.GetRolesAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<string> { AppRole.SiteMaintenance });
        var page = new EditRolesModel(staffServiceMock.Object)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsGlobal.GetPageTempData(),
        };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            page.ModelState.IsValid.Should().BeTrue();
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(Guid.Empty.ToString());
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        }
    }

    [Test]
    public async Task OnPost_GivenMissingUser_ReturnsBadRequest()
    {
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.UpdateRolesAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, bool>>()))
            .ReturnsAsync(IdentityResult.Failed());
        staffServiceMock.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync((StaffViewDto?)null);
        var page = new EditRolesModel(staffServiceMock.Object)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsGlobal.GetPageTempData(),
        };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenUpdateFailure_ReturnsPageWithInvalidModelState()
    {
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.UpdateRolesAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, bool>>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "CODE", Description = "DESCRIPTION" }));
        staffServiceMock.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync(StaffViewTest);
        staffServiceMock.Setup(l => l.GetRolesAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<string> { AppRole.SiteMaintenance });
        var page = new EditRolesModel(staffServiceMock.Object)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsGlobal.GetPageTempData(),
        };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
            page.ModelState[string.Empty]!.Errors[0].ErrorMessage.Should().Be("CODE: DESCRIPTION");
            page.DisplayStaff.Should().Be(StaffViewTest);
            page.UserId.Should().Be(Guid.Empty.ToString());
            page.RoleSettings.Should().BeEquivalentTo(RoleSettingsTest);
        }
    }
}
