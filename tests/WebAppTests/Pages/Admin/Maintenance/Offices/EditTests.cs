using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Admin.Maintenance.Offices;
using Cts.WebApp.Platform.Models;
using Cts.WebApp.Platform.PageModelHelpers;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Admin.Maintenance.Offices;

public class EditTests
{
    private static readonly OfficeUpdateDto ItemTest = new() { Id = Guid.Empty, Name = TestConstants.ValidName };

    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var serviceMock = new Mock<IOfficeAppService>();
        serviceMock.Setup(l => l.FindForUpdateAsync(ItemTest.Id, CancellationToken.None)).ReturnsAsync(ItemTest);
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.GetStaffListItemsAsync(It.IsAny<bool>()))
            .ReturnsAsync(new List<ListItem<string>>());
        var page = new EditModel(serviceMock.Object, staffServiceMock.Object, Mock.Of<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsGlobal.PageTempData() };

        await page.OnGetAsync(ItemTest.Id);

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
        var serviceMock = new Mock<IOfficeAppService>();
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.GetStaffListItemsAsync(It.IsAny<bool>()))
            .ReturnsAsync(new List<ListItem<string>>());
        var page = new EditModel(serviceMock.Object, staffServiceMock.Object, Mock.Of<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsGlobal.PageTempData() };

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
        var serviceMock = new Mock<IOfficeAppService>();
        serviceMock.Setup(l => l.FindForUpdateAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((OfficeUpdateDto?)null);
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.GetStaffListItemsAsync(It.IsAny<bool>()))
            .ReturnsAsync(new List<ListItem<string>>());
        var page = new EditModel(serviceMock.Object, staffService.Object, Mock.Of<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnGetAsync(Guid.Empty);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var serviceMock = new Mock<IOfficeAppService>();
        var staffServiceMock = new Mock<IStaffAppService>();
        var validatorMock = new Mock<IValidator<OfficeUpdateDto>>();
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<OfficeUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        var page = new EditModel(serviceMock.Object, staffServiceMock.Object, validatorMock.Object)
            { Item = ItemTest, TempData = WebAppTestsGlobal.PageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"“{ItemTest.Name}” successfully updated.");

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            page.HighlightId.Should().Be(ItemTest.Id);
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        var serviceMock = new Mock<IOfficeAppService>();
        var staffServiceMock = new Mock<IStaffAppService>();
        staffServiceMock.Setup(l => l.GetStaffListItemsAsync(It.IsAny<bool>()))
            .ReturnsAsync(new List<ListItem<string>>());
        var validatorMock = new Mock<IValidator<OfficeUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.Setup(l => l.ValidateAsync(It.IsAny<OfficeUpdateDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult(validationFailures));
        var page = new EditModel(serviceMock.Object, staffServiceMock.Object, validatorMock.Object)
            { Item = ItemTest, TempData = WebAppTestsGlobal.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
