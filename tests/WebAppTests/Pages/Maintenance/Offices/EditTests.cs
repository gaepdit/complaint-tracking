using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.WebApp.Pages.Admin.Maintenance.Offices;

namespace WebAppTests.Pages.Maintenance.Offices;

public class EditTests
{
    private static readonly OfficeUpdateDto ItemTest = new(TextData.ValidName, true);

    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        // Arrange
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.FindForUpdateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(ItemTest);

        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetAsListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());

        var page = new EditModel(officeServiceMock, staffServiceMock, Substitute.For<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData(), Id = TextData.TestGuid };

        // Act
        await page.OnGetAsync();

        // Assert
        using var scope = new AssertionScope();
        page.Item.Should().BeEquivalentTo(ItemTest);
        page.OriginalName.Should().Be(ItemTest.Name);
        page.HighlightId.Should().Be(Guid.Empty);
    }

    [Test]
    public async Task OnGet_GivenInvalidId_ReturnsNotFound()
    {
        // Arrange
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.FindForUpdateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((OfficeUpdateDto?)null);

        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetAsListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());

        var page = new EditModel(officeServiceMock, staffServiceMock, Substitute.For<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData(), Id = TextData.TestGuid };

        // Act
        var result = await page.OnGetAsync();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        // Arrange
        var validatorMock = Substitute.For<IValidator<OfficeUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var page = new EditModel(Substitute.For<IOfficeService>(), Substitute.For<IStaffService>(), validatorMock)
            { Id = Guid.NewGuid(), Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"“{ItemTest.Name}” successfully updated.", []);

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        page.HighlightId.Should().Be(page.Id);
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        // Arrange
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetAsListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());

        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        var validatorMock = Substitute.For<IValidator<OfficeUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        var page = new EditModel(Substitute.For<IOfficeService>(), staffServiceMock, validatorMock)
            { Id = Guid.Empty, Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
    }
}
