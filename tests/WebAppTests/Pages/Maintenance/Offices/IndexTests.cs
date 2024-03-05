using Cts.AppServices.Offices;
using Cts.WebApp.Pages.Admin.Maintenance.Offices;

namespace WebAppTests.Pages.Maintenance.Offices;

public class IndexTests
{
    private static readonly List<OfficeWithAssignorDto> ListTest =
        [new OfficeWithAssignorDto(Guid.Empty, TextData.ValidName, true)];

    [Test]
    public async Task OnGet_ReturnsWithList()
    {
        var serviceMock = Substitute.For<IOfficeService>();
        serviceMock.GetListIncludeAssignorAsync(Arg.Any<CancellationToken>()).Returns(ListTest);
        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());
        var page = new IndexModel { TempData = WebAppTestsSetup.PageTempData() };

        await page.OnGetAsync(serviceMock, authorizationMock);

        using var scope = new AssertionScope();
        page.Items.Should().BeEquivalentTo(ListTest);
        page.TempData.GetDisplayMessage().Should().BeNull();
        page.HighlightId.Should().BeNull();
    }
}
