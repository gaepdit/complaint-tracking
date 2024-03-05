using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.WebApp.Pages.Public.Complaints;

namespace WebAppTests.Pages.PublicComplaints;

public class IndexPageTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        // Arrange
        var item = new ComplaintPublicViewDto();

        var serviceMock = Substitute.For<IComplaintService>();
        serviceMock.FindPublicAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(item);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Failed());

        var pageModel = new IndexModel(serviceMock, authorizationMock);

        // Act
        var result = await pageModel.OnGetAsync(1);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        pageModel.Item.Should().Be(item);
        pageModel.UserIsActive.Should().BeFalse();
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        // Arrange
        var serviceMock = Substitute.For<IComplaintService>();

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Failed());

        var pageModel = new IndexModel(serviceMock, authorizationMock);

        // Act
        var result = await pageModel.OnGetAsync(null);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("../Index");
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        // Arrange
        var serviceMock = Substitute.For<IComplaintService>();

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Failed());

        var pageModel = new IndexModel(serviceMock, authorizationMock);

        // Act
        var result = await pageModel.OnGetAsync(0);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
