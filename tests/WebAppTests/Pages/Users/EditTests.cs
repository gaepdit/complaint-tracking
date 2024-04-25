using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.WebApp.Pages.Admin.Users;

namespace WebAppTests.Pages.Users;

public class EditTests
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

    private static readonly StaffUpdateDto StaffUpdateTest = new() { Active = true };

    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns(StaffViewTest);

        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetAsListItemsAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(new List<ListItem>());

        var pageModel = new EditModel(staffServiceMock, officeServiceMock,
                Substitute.For<IValidator<StaffUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await pageModel.OnGetAsync(StaffViewTest.Id);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        pageModel.DisplayStaff.Should().Be(StaffViewTest);
        pageModel.Item.Should().BeEquivalentTo(StaffViewTest.AsUpdateDto());
        pageModel.OfficesSelectList.Should().BeEmpty();
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        // Arrange
        var pageModel = new EditModel(Substitute.For<IStaffService>(),
                Substitute.For<IOfficeService>(), Substitute.For<IValidator<StaffUpdateDto>>())
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
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns((StaffViewDto?)null);

        var pageModel = new EditModel(staffServiceMock, Substitute.For<IOfficeService>(),
                Substitute.For<IValidator<StaffUpdateDto>>())
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
            new DisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated.", []);

        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateAsync(Arg.Any<string>(), Arg.Any<StaffUpdateDto>()).Returns(IdentityResult.Success);

        var validatorMock = Substitute.For<IValidator<StaffUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<StaffUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var page = new EditModel(staffServiceMock, Substitute.For<IOfficeService>(), validatorMock)
            { Item = StaffUpdateTest, TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        page.ModelState.IsValid.Should().BeTrue();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Details");
        ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(Guid.Empty);
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
    }

    [Test]
    public async Task OnPost_GivenUpdateFailure_ReturnsBadRequest()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateAsync(Arg.Any<string>(), Arg.Any<StaffUpdateDto>()).Returns(IdentityResult.Failed());

        var validatorMock = Substitute.For<IValidator<StaffUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<StaffUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var page = new EditModel(staffServiceMock, Substitute.For<IOfficeService>(), validatorMock)
            { Item = StaffUpdateTest, TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenInvalidModel_ReturnsPageWithInvalidModelState()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns(StaffViewTest);

        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetAsListItemsAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(new List<ListItem>());

        var validatorMock = Substitute.For<IValidator<StaffUpdateDto>>();

        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.ValidateAsync(Arg.Any<StaffUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        var page = new EditModel(staffServiceMock, officeServiceMock, validatorMock)
            { Item = StaffUpdateTest, TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
        page.DisplayStaff.Should().Be(StaffViewTest);
        page.Item.Should().Be(StaffUpdateTest);
    }

    [Test]
    public async Task OnPost_GivenMissingUser_ReturnsBadRequest()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns((StaffViewDto?)null);

        var validationFailures = new List<ValidationFailure> { new("property", "message") };

        var validatorMock = Substitute.For<IValidator<StaffUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<StaffUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        var page = new EditModel(staffServiceMock, Substitute.For<IOfficeService>(), validatorMock)
            { Item = StaffUpdateTest, TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
