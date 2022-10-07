using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAppRoot.AppServices.Offices;
using MyAppRoot.TestData.Constants;
using MyAppRoot.WebApp.Models;
using MyAppRoot.WebApp.Pages.Admin.Maintenance.Offices;
using MyAppRoot.WebApp.Platform.RazorHelpers;

namespace WebAppTests.Pages.Admin.Maintenance.Offices;

public class EditTests
{
    private static readonly OfficeUpdateDto ItemTest = new() { Id = Guid.Empty, Name = TestConstants.ValidName };

    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var service = new Mock<IOfficeAppService>();
        service.Setup(l => l.FindForUpdateAsync(ItemTest.Id, CancellationToken.None)).ReturnsAsync(ItemTest);
        var page = new EditModel(service.Object, Mock.Of<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsGlobal.GetPageTempData() };

        await page.OnGetAsync(ItemTest.Id);

        Assert.Multiple(() =>
        {
            page.Item.Should().BeEquivalentTo(ItemTest);
            page.OriginalName.Should().Be(ItemTest.Name);
            page.HighlightId.Should().Be(Guid.Empty);
        });
    }

    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var service = new Mock<IOfficeAppService>();
        var page = new EditModel(service.Object, Mock.Of<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnGetAsync(null);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnGet_GivenInvalidId_ReturnsNotFound()
    {
        var service = new Mock<IOfficeAppService>();
        service.Setup(l => l.FindForUpdateAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((OfficeUpdateDto?)null);
        var page = new EditModel(service.Object, Mock.Of<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnGetAsync(Guid.Empty);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)result).Value.Should().Be("ID not found.");
        });
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var service = new Mock<IOfficeAppService>();
        var validator = new Mock<IValidator<OfficeUpdateDto>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<OfficeUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new EditModel(service.Object, validator.Object)
            { Item = ItemTest, TempData = WebAppTestsGlobal.GetPageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"\"{ItemTest.Name}\" successfully updated.");

        var result = await page.OnPostAsync();

        Assert.Multiple(() =>
        {
            page.HighlightId.Should().Be(ItemTest.Id);
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        });
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        var service = new Mock<IOfficeAppService>();
        var validator = new Mock<IValidator<OfficeUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validator.Setup(l => l.ValidateAsync(It.IsAny<OfficeUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new EditModel(service.Object, validator.Object)
            { Item = ItemTest, TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnPostAsync();

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        });
    }
}
