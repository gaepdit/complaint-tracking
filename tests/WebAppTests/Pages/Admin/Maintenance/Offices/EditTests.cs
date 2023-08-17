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
        var serviceMock = Substitute.For<IOfficeService>();
        serviceMock.FindForUpdateAsync(ItemTest.Id, Arg.Any<CancellationToken>()).Returns(ItemTest);
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetStaffListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());
        var page = new EditModel(serviceMock, staffServiceMock, Substitute.For<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

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
        var serviceMock = Substitute.For<IOfficeService>();
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetStaffListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());
        var page = new EditModel(serviceMock, staffServiceMock, Substitute.For<IValidator<OfficeUpdateDto>>())
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
        var serviceMock = Substitute.For<IOfficeService>();
        serviceMock.FindForUpdateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((OfficeUpdateDto?)null);
        var staffService = Substitute.For<IStaffService>();
        staffService.GetStaffListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());
        var page = new EditModel(serviceMock, staffService, Substitute.For<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnGetAsync(Guid.Empty);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var serviceMock = Substitute.For<IOfficeService>();
        var staffServiceMock = Substitute.For<IStaffService>();
        var validatorMock = Substitute.For<IValidator<OfficeUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<OfficeUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        var page = new EditModel(serviceMock, staffServiceMock, validatorMock)
            { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };
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
        var serviceMock = Substitute.For<IOfficeService>();
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetStaffListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());
        var validatorMock = Substitute.For<IValidator<OfficeUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.ValidateAsync(Arg.Any<OfficeUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));
        var page = new EditModel(serviceMock, staffServiceMock, validatorMock)
            { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
