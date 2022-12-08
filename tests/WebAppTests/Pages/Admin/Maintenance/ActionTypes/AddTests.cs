using Cts.AppServices.ActionTypes;
using Cts.TestData.Constants;
using Cts.WebApp.Models;
using Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;
using Cts.WebApp.Platform.RazorHelpers;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Admin.Maintenance.ActionTypes;

public class AddTests
{
    private static readonly ActionTypeCreateDto ItemTest = new() { Name = TestConstants.ValidName };

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var service = new Mock<IActionTypeAppService>();
        service.Setup(l => l.CreateAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(Guid.Empty);
        var validator = new Mock<IValidator<ActionTypeCreateDto>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<ActionTypeCreateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new AddModel { Item = ItemTest, TempData = WebAppTestsGlobal.GetPageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"\"{ItemTest.Name}\" successfully added.");

        var result = await page.OnPostAsync(service.Object, validator.Object);

        using (new AssertionScope())
        {
            page.HighlightId.Should().Be(Guid.Empty);
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        var service = new Mock<IActionTypeAppService>();
        var validator = new Mock<IValidator<ActionTypeCreateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validator.Setup(l => l.ValidateAsync(It.IsAny<ActionTypeCreateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new AddModel { Item = ItemTest, TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnPostAsync(service.Object, validator.Object);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
