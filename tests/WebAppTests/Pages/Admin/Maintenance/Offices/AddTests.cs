using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.WebApp.Pages.Admin.Maintenance.Offices;

namespace WebAppTests.Pages.Admin.Maintenance.Offices;

public class AddTests
{
    private static readonly OfficeCreateDto ItemTest = new(TextData.ValidName);

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.CreateAsync(Arg.Any<OfficeCreateDto>(), Arg.Any<CancellationToken>())
            .Returns(Guid.Empty);
        var staffServiceMock = Substitute.For<IStaffService>();
        var validatorMock = Substitute.For<IValidator<OfficeCreateDto>>();
        validatorMock.ValidateAsync(Arg.Any<OfficeCreateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        var page = new AddModel(officeServiceMock, staffServiceMock, validatorMock)
            { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"“{ItemTest.Name}” successfully added.");

        var result = await page.OnPostAsync();

        using var scope = new AssertionScope();
        page.HighlightId.Should().Be(Guid.Empty);
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        var officeServiceMock = Substitute.For<IOfficeService>();
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetStaffListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());
        var validatorMock = Substitute.For<IValidator<OfficeCreateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.ValidateAsync(Arg.Any<OfficeCreateDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));
        var page = new AddModel(officeServiceMock, staffServiceMock, validatorMock)
            { Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
    }
}
