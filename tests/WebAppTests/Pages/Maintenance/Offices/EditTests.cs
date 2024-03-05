using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.WebApp.Pages.Admin.Maintenance.Offices;

namespace WebAppTests.Pages.Maintenance.Offices;

public class EditTests
{
    private static readonly OfficeUpdateDto ItemTest = new(TextData.ValidName, true);

    [Test]
    public async Task OnGet_ReturnsWithItem()
    {
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.FindForUpdateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(ItemTest);
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetAsListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());
        var page = new EditModel(officeServiceMock, staffServiceMock, Substitute.For<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        await page.OnGetAsync(Guid.Empty);

        using var scope = new AssertionScope();
        page.Item.Should().BeEquivalentTo(ItemTest);
        page.OriginalName.Should().Be(ItemTest.Name);
        page.HighlightId.Should().Be(Guid.Empty);
    }

    [Test]
    public async Task OnGet_GivenNullId_ReturnsNotFound()
    {
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetAsListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());
        var page = new EditModel(Substitute.For<IOfficeService>(), staffServiceMock,
                Substitute.For<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnGetAsync(null);

        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnGet_GivenInvalidId_ReturnsNotFound()
    {
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.FindForUpdateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((OfficeUpdateDto?)null);
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetAsListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());
        var page = new EditModel(officeServiceMock, staffServiceMock, Substitute.For<IValidator<OfficeUpdateDto>>())
            { TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnGetAsync(Guid.Empty);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var validatorMock = Substitute.For<IValidator<OfficeUpdateDto>>();
        validatorMock.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        var page = new EditModel(Substitute.For<IOfficeService>(), Substitute.For<IStaffService>(), validatorMock)
            { Id = Guid.NewGuid(), Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, $"“{ItemTest.Name}” successfully updated.");

        var result = await page.OnPostAsync();

        using var scope = new AssertionScope();
        page.HighlightId.Should().Be(page.Id);
        page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnPost_GivenInvalidItem_ReturnsPageWithModelErrors()
    {
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.GetAsListItemsAsync(Arg.Any<bool>())
            .Returns(new List<ListItem<string>>());
        var validatorMock = Substitute.For<IValidator<OfficeUpdateDto>>();
        var validationFailures = new List<ValidationFailure> { new("property", "message") };
        validatorMock.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));
        var page = new EditModel(Substitute.For<IOfficeService>(), staffServiceMock, validatorMock)
            { Id = Guid.Empty, Item = ItemTest, TempData = WebAppTestsSetup.PageTempData() };

        var result = await page.OnPostAsync();

        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
    }
}
