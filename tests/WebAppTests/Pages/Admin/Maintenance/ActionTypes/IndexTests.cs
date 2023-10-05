using Cts.AppServices.ActionTypes;
using Cts.TestData.Constants;
using Cts.WebApp.Pages.Admin.Maintenance.ActionTypes;
using Cts.WebApp.Platform.PageModelHelpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebAppTests.Pages.Admin.Maintenance.ActionTypes;

public class IndexTests
{
    private static readonly List<ActionTypeViewDto> ListTest = new()
        { new ActionTypeViewDto(Guid.Empty, TextData.ValidName, true) };

    [Test]
    public async Task OnGet_ReturnsWithList()
    {
        var serviceMock = Substitute.For<IActionTypeService>();
        serviceMock.GetListAsync(CancellationToken.None).Returns(ListTest);
        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Is((string?)null), Arg.Any<string>())
            .Returns(AuthorizationResult.Success());
        var page = new IndexModel
        {
            TempData = WebAppTestsSetup.PageTempData(),
            PageContext = WebAppTestsSetup.PageContextWithUser(),
        };

        await page.OnGetAsync(serviceMock, authorizationMock);

        using (new AssertionScope())
        {
            page.Items.Should().BeEquivalentTo(ListTest);
            page.TempData.GetDisplayMessage().Should().BeNull();
            page.HighlightId.Should().BeNull();
        }
    }
}
