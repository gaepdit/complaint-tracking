using Cts.AppServices.ActionTypes;
using Cts.TestData.ActionTypes;
using Cts.WebApp.Models;
using Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;
using Cts.WebApp.Platform.RazorHelpers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Admin.Maintenance.ActionTypes;

public class EditTests
{
    private readonly ActionTypeUpdateDto _item = new() { Id = Guid.Empty, Name = ActionTypeConstants.ValidName };

    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var service = new Mock<IActionTypeAppService>();
        service.Setup(l => l.FindForUpdateAsync(_item.Id, CancellationToken.None)).ReturnsAsync(_item);
        var page = new Edit(service.Object, Mock.Of<IValidator<ActionTypeUpdateDto>>())
            { TempData = WebAppTestsGlobal.GetPageTempData() };

        await page.OnGetAsync(_item.Id);

        Assert.Multiple(() =>
        {
            page.Item.Should().BeEquivalentTo(_item);
            page.OriginalName.Should().Be(_item.Name);
            page.HighlightId.Should().Be(Guid.Empty);
        });
    }

    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var service = new Mock<IActionTypeAppService>();
        var page = new Edit(service.Object, Mock.Of<IValidator<ActionTypeUpdateDto>>())
            { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnGetAsync(null);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<NotFoundResult>();
            page.Item.Should().BeNull();
        });
    }

    [Test]
    public async Task OnGet_GivenInvalidId_ReturnsNotFound()
    {
        var service = new Mock<IActionTypeAppService>();
        service.Setup(l => l.FindForUpdateAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((ActionTypeUpdateDto?)null);
        var page = new Edit(service.Object, Mock.Of<IValidator<ActionTypeUpdateDto>>())
            { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnGetAsync(Guid.Empty);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)result).Value.Should().Be("ID not found.");
            page.Item.Should().BeNull();
        });
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var service = new Mock<IActionTypeAppService>();
        var validator = new Mock<IValidator<ActionTypeUpdateDto>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<ActionTypeUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new Edit(service.Object, validator.Object)
            { Item = _item, TempData = WebAppTestsGlobal.GetPageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"\"{_item.Name}\" successfully updated.");

        var result = await page.OnPostAsync();

        Assert.Multiple(() =>
        {
            page.HighlightId.Should().Be(_item.Id);
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        });
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        var service = new Mock<IActionTypeAppService>();
        var validator = new Mock<IValidator<ActionTypeUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validator.Setup(l => l.ValidateAsync(It.IsAny<ActionTypeUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new Edit(service.Object, validator.Object)
            { Item = _item, TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnPostAsync();

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        });
    }
}
