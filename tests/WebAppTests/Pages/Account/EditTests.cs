using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Account;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Account;

public class EditTests
{
    private static readonly StaffViewDto StaffViewTest = new()
    {
        Id = Guid.Empty.ToString(),
        Email = TestConstants.ValidEmail,
        GivenName = TestConstants.ValidName,
        FamilyName = TestConstants.ValidName,
    };

    private static readonly StaffUpdateDto StaffUpdateTest = new()
    {
        Id = Guid.Empty.ToString(),
        Active = true,
    };

    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync(StaffViewTest);
        var officeService = new Mock<IOfficeAppService>();
        officeService.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var pageModel = new EditModel(staffService.Object, officeService.Object, Mock.Of<IValidator<StaffUpdateDto>>())
        { TempData = WebAppTestsGlobal.PageTempData() };

        var result = await pageModel.OnGetAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(StaffViewTest);
            pageModel.UpdateStaff.Should().BeEquivalentTo(StaffViewTest.AsUpdateDto());
            pageModel.OfficeItems.Should().BeEmpty();
        }
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated profile.");

        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync(StaffViewTest);
        staffService.Setup(l => l.UpdateAsync(It.IsAny<StaffUpdateDto>()))
            .ReturnsAsync(IdentityResult.Success);
        var validator = new Mock<IValidator<StaffUpdateDto>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new EditModel(staffService.Object, Mock.Of<IOfficeAppService>(), validator.Object)
        { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            page.ModelState.IsValid.Should().BeTrue();
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        }
    }

    [Test]
    public async Task OnPost_GivenUpdateFailure_ReturnsBadRequest()
    {
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync(StaffViewTest);
        staffService.Setup(l => l.UpdateAsync(It.IsAny<StaffUpdateDto>()))
            .ReturnsAsync(IdentityResult.Failed());
        var validator = new Mock<IValidator<StaffUpdateDto>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new EditModel(staffService.Object, Mock.Of<IOfficeAppService>(), validator.Object)
        { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenInvalidModel_ReturnsPageWithInvalidModelState()
    {
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync(StaffViewTest);
        var officeService = new Mock<IOfficeAppService>();
        officeService.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var validator = new Mock<IValidator<StaffUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validator.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var page = new EditModel(staffService.Object, officeService.Object, validator.Object)
        { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
            page.DisplayStaff.Should().Be(StaffViewTest);
            page.UpdateStaff.Should().Be(StaffUpdateTest);
        }
    }
}
