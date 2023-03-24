using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Admin.Users;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Admin.Users;

public class EditTests
{
    private static readonly StaffViewDto StaffViewTest = new()
    {
        Id = Guid.Empty.ToString(),
        Email = TestConstants.ValidEmail,
        GivenName = TestConstants.ValidName,
        FamilyName = TestConstants.ValidName,
    };

    private static readonly StaffUpdateDto StaffUpdateTest = new() { Id = Guid.Empty.ToString() };

    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync(StaffViewTest);
        var officeServiceMock = new Mock<IOfficeAppService>();
        officeServiceMock.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var pageModel = new EditModel(staffServiceMock.Object, officeServiceMock.Object, Mock.Of<IValidator<StaffUpdateDto>>())
        { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(StaffViewTest.Id);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(StaffViewTest);
            pageModel.UpdateStaff.Should().BeEquivalentTo(StaffViewTest.AsUpdateDto());
            pageModel.OfficeItems.Should().BeEmpty();
        }
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var pageModel = new EditModel(Mock.Of<IStaffAppService>(),
            Mock.Of<IOfficeAppService>(), Mock.Of<IValidator<StaffUpdateDto>>())
        { TempData = WebAppTestsGlobal.GetPageTempData() };

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
        var pageModel = new EditModel(staffServiceMock.Object,
            Mock.Of<IOfficeAppService>(), Mock.Of<IValidator<StaffUpdateDto>>())
        { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(Guid.Empty.ToString());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated.");

        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.UpdateAsync(It.IsAny<StaffUpdateDto>()))
            .ReturnsAsync(IdentityResult.Success);
        var validatorMock = new Mock<IValidator<StaffUpdateDto>>();
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new EditModel(staffServiceMock.Object, Mock.Of<IOfficeAppService>(), validatorMock.Object)
        { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.GetPageTempData() };

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
    public async Task OnPost_GivenUpdateFailure_ReturnsBadRequest()
    {
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.UpdateAsync(It.IsAny<StaffUpdateDto>()))
            .ReturnsAsync(IdentityResult.Failed());
        var validator = new Mock<IValidator<StaffUpdateDto>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new EditModel(staffService.Object, Mock.Of<IOfficeAppService>(), validator.Object)
        { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenInvalidModel_ReturnsPageWithInvalidModelState()
    {
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync(StaffViewTest);
        var officeServiceMock = new Mock<IOfficeAppService>();
        officeServiceMock.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var validatorMock = new Mock<IValidator<StaffUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var page = new EditModel(staffServiceMock.Object, officeServiceMock.Object, validatorMock.Object)
        { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
            page.DisplayStaff.Should().Be(StaffViewTest);
            page.UpdateStaff.Should().Be(StaffUpdateTest);
        }
    }

    [Test]
    public async Task OnPost_GivenMissingUser_ReturnsBadRequest()
    {
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync((StaffViewDto?)null);
        var validatorMock = new Mock<IValidator<StaffUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new EditModel(staffServiceMock.Object, Mock.Of<IOfficeAppService>(), validatorMock.Object)
        { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }
}
