using FluentValidation;
using FluentValidation.Results;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAppRoot.AppServices.Offices;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.TestData.Constants;
using MyAppRoot.WebApp.Models;
using MyAppRoot.WebApp.Pages.Admin.Users;
using MyAppRoot.WebApp.Platform.RazorHelpers;

namespace WebAppTests.Pages.Admin.Users;

public class EditTests
{
    private static readonly StaffViewDto StaffViewTest = new()
    {
        Id = Guid.Empty,
        Email = TestConstants.ValidEmail,
        FirstName = TestConstants.ValidName,
        LastName = TestConstants.ValidName,
    };

    private static readonly StaffUpdateDto StaffUpdateTest = new() { Id = Guid.Empty };

    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(StaffViewTest);
        var officeService = new Mock<IOfficeAppService>();
        officeService.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var validator = new Mock<IValidator<StaffUpdateDto>>();
        var pageModel = new EditModel(staffService.Object, officeService.Object, validator.Object)
            { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(StaffViewTest.Id);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(StaffViewTest);
            pageModel.UpdateStaff.Should().BeEquivalentTo(StaffViewTest.AsUpdateDto());
            pageModel.OfficeItems.Should().BeEmpty();
        });
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var staffService = new Mock<IStaffAppService>();
        var officeService = new Mock<IOfficeAppService>();
        var validator = new Mock<IValidator<StaffUpdateDto>>();
        var pageModel = new EditModel(staffService.Object, officeService.Object, validator.Object)
            { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(null);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((StaffViewDto?)null);
        var officeService = new Mock<IOfficeAppService>();
        var validator = new Mock<IValidator<StaffUpdateDto>>();
        var pageModel = new EditModel(staffService.Object, officeService.Object, validator.Object)
            { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(Guid.Empty);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)result).Value.Should().Be("ID not found.");
        });
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated.");

        var staffService = new Mock<IStaffAppService>();
        var officeService = new Mock<IOfficeAppService>();
        var validator = new Mock<IValidator<StaffUpdateDto>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new EditModel(staffService.Object, officeService.Object, validator.Object)
            { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnPostAsync();

        Assert.Multiple(() =>
        {
            page.ModelState.IsValid.Should().BeTrue();
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(Guid.Empty);
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        });
    }

    [Test]
    public async Task OnPost_GivenInvalidModel_ReturnsPageWithInvalidModelState()
    {
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync(StaffViewTest);
        var officeService = new Mock<IOfficeAppService>();
        officeService.Setup(l => l.GetActiveListItemsAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem>());
        var validator = new Mock<IValidator<StaffUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validator.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var page = new EditModel(staffService.Object, officeService.Object, validator.Object)
            { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnPostAsync();

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
            page.DisplayStaff.Should().Be(StaffViewTest);
            page.UpdateStaff.Should().Be(StaffUpdateTest);
        });
    }

    [Test]
    public async Task OnPost_GivenMissingUser_ReturnsBadRequest()
    {
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.FindAsync(It.IsAny<Guid>()))
            .ReturnsAsync((StaffViewDto?)null);
        var officeService = new Mock<IOfficeAppService>();
        var validator = new Mock<IValidator<StaffUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validator.Setup(l => l.ValidateAsync(It.IsAny<StaffUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new EditModel(staffService.Object, officeService.Object, validator.Object)
            { UpdateStaff = StaffUpdateTest, TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }
}
