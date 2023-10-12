using Cts.AppServices.ActionTypes;
using Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;

namespace WebAppTests.Pages.Admin.Maintenance.ActionTypes;

public class AddTests
{
    private static readonly ActionTypeCreateDto ItemTest = new(TextData.ValidName);

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var serviceMock = Substitute.For<IActionTypeService>();
        serviceMock.CreateAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Guid.Empty);
        var validatorMock = Substitute.For<IValidator<ActionTypeCreateDto>>();
        validatorMock.ValidateAsync(Arg.Any<ActionTypeCreateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        var page = new AddModel { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"“{ItemTest.Name}” successfully added.");

        var result = await page.OnPostAsync(serviceMock, validatorMock);

        using var scope = new AssertionScope();
        page.HighlightId.Should().Be(Guid.Empty);
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        var serviceMock = Substitute.For<IActionTypeService>();
        var validatorMock = Substitute.For<IValidator<ActionTypeCreateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.ValidateAsync(Arg.Any<ActionTypeCreateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));
        var page = new AddModel { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync(serviceMock, validatorMock);

        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
    }
}
