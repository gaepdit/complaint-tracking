using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Admin.Maintenance.Offices;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageDisplayHelpers;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.GetActiveStaffMembersAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem<string>>());
        var validator = new Mock<IValidator<OfficeCreateDto>>();
        validator.Setup(l => l.ValidateAsync(It.IsAny<OfficeCreateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new AddModel(service.Object, staffService.Object, validator.Object)
            { Item = ItemTest, TempData = WebAppTestsGlobal.GetPageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"“{ItemTest.Name}” successfully added.");

        var result = await page.OnPostAsync();

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
        var service = new Mock<IOfficeAppService>();
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.GetActiveStaffMembersAsync(CancellationToken.None))
            .ReturnsAsync(new List<ListItem<string>>());
        var validator = new Mock<IValidator<OfficeCreateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validator.Setup(l => l.ValidateAsync(It.IsAny<OfficeCreateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new AddModel(service.Object, staffService.Object, validator.Object)
            { Item = ItemTest, TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
