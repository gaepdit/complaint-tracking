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

public class AddTests
{
    private static readonly OfficeCreateDto ItemTest = new() { Name = TestConstants.ValidName };

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var service = new Mock<IOfficeAppService>();
        service.Setup(l => l.CreateAsync(It.IsAny<OfficeCreateDto>(), CancellationToken.None))
            .ReturnsAsync(Guid.Empty);
        var validator = new Mock<IValidator<OfficeCreateDto>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<OfficeCreateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new AddModel { Item = ItemTest, TempData = WebAppTestsGlobal.GetPageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"\"{ItemTest.Name}\" successfully added.");

        var result = await page.OnPostAsync(service.Object, validator.Object);

        Assert.Multiple(() =>
        {
            page.HighlightId.Should().Be(Guid.Empty);
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        });
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        var service = new Mock<IOfficeAppService>();
        var validator = new Mock<IValidator<OfficeCreateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validator.Setup(l => l.ValidateAsync(It.IsAny<OfficeCreateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new AddModel { Item = ItemTest, TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnPostAsync(service.Object, validator.Object);

        Assert.Multiple(() =>
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        });
    }
}
