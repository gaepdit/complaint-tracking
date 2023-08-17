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

public class AddTests
{
    private static readonly OfficeCreateDto ItemTest = new() { Name = TestConstants.ValidName };

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var serviceMock = Substitute.For<IOfficeService>();
        serviceMock.CreateAsync(Arg.Any<OfficeCreateDto>(), Arg.Any<CancellationToken>())
            .Returns(Guid.Empty);
        var staffService = Substitute.For<IStaffService>();
        staffService.GetStaffListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());
        var validator = Substitute.For<IValidator<OfficeCreateDto>>();
        validator.ValidateAsync(Arg.Any<OfficeCreateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        var page = new AddModel(serviceMock, staffService, validator)
            { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };
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
        var serviceMock = Substitute.For<IOfficeService>();
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetStaffListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());
        var validator = Substitute.For<IValidator<OfficeCreateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validator.ValidateAsync(Arg.Any<OfficeCreateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));
        var page = new AddModel(serviceMock, staffServiceMock, validator)
            { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
