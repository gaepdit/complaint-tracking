using Cts.AppServices.ActionTypes;
using Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;

namespace WebAppTests.Pages.Maintenance.ActionTypes;

public class EditTests
{
    private static readonly ActionTypeUpdateDto ItemTest = new(TextData.ValidName, true);

    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        // Arrange
        var serviceMock = Substitute.For<IActionTypeService>();
        serviceMock.FindForUpdateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(ItemTest);

        var page = new EditModel(serviceMock, Substitute.For<IValidator<ActionTypeUpdateDto>>())
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
        var serviceMock = Substitute.For<IActionTypeService>();
        serviceMock.FindForUpdateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((ActionTypeUpdateDto?)null);

        var page = new EditModel(serviceMock, Substitute.For<IValidator<ActionTypeUpdateDto>>())
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
        var serviceMock = Substitute.For<IActionTypeService>();

        var validatorMock = Substitute.For<IValidator<ActionTypeUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var page = new EditModel(serviceMock, validatorMock)
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
        var serviceMock = Substitute.For<IActionTypeService>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };

        var validatorMock = Substitute.For<IValidator<ActionTypeUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        var page = new EditModel(serviceMock, validatorMock)
            { Id = Guid.Empty, Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
    }
}
