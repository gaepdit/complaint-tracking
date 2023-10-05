using Cts.AppServices.ActionTypes;
using Cts.TestData.Constants;
using Cts.WebApp.Models;
using Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Admin.Maintenance.ActionTypes;

public class EditTests
{
    private static readonly ActionTypeUpdateDto ItemTest = new(TextData.ValidName, true);

    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var serviceMock = Substitute.For<IActionTypeService>();
        serviceMock.FindForUpdateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(ItemTest);
        var page = new EditModel(serviceMock, Substitute.For<IValidator<ActionTypeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        await page.OnGetAsync(Guid.Empty);

        using (new AssertionScope())
        {
            page.Item.Should().BeEquivalentTo(ItemTest);
            page.OriginalName.Should().Be(ItemTest.Name);
            page.HighlightId.Should().Be(Guid.Empty);
        }
    }

    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var serviceMock = Substitute.For<IActionTypeService>();
        var page = new EditModel(serviceMock, Substitute.For<IValidator<ActionTypeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnGetAsync(null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnGet_GivenInvalidId_ReturnsNotFound()
    {
        var serviceMock = Substitute.For<IActionTypeService>();
        serviceMock.FindForUpdateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((ActionTypeUpdateDto?)null);
        var page = new EditModel(serviceMock, Substitute.For<IValidator<ActionTypeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnGetAsync(Guid.Empty);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var serviceMock = Substitute.For<IActionTypeService>();
        var validatorMock = Substitute.For<IValidator<ActionTypeUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        var page = new EditModel(serviceMock, validatorMock)
            { Id = Guid.NewGuid(), Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"“{ItemTest.Name}” successfully updated.");

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            page.HighlightId.Should().Be(page.Id);
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        var serviceMock = Substitute.For<IActionTypeService>();
        var validatorMock = Substitute.For<IValidator<ActionTypeUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));
        var page = new EditModel(serviceMock, validatorMock)
            { Id = Guid.Empty, Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
