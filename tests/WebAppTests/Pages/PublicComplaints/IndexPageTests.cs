using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.WebApp.Pages;

namespace WebAppTests.Pages.PublicComplaints;

public class IndexPageTests
{
    [Test]
    public async Task OnGet_ValidId_PopulatesThePageModel()
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

        var pageModel = new ComplaintModel(serviceMock, authorizationMock) { Id = 1 };

        // Act
        var result = await pageModel.OnGetAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        pageModel.Item.Should().Be(item);
        pageModel.UserIsActive.Should().BeFalse();
    }

    [Test]
    public async Task OnGet_InvalidId_RedirectsToIndexPage()
    {
        // Arrange
        var serviceMock = Substitute.For<IComplaintService>();

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Failed());

        var pageModel = new ComplaintModel(serviceMock, authorizationMock) { Id = -1 };

        // Act
        var result = await pageModel.OnGetAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("Index");
    }

    [Test]
    public async Task OnGet_NonexistentId_ReturnsNotFound()
    {
        // Arrange
        var serviceMock = Substitute.For<IComplaintService>();

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(user: Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Failed());

        var pageModel = new ComplaintModel(serviceMock, authorizationMock) { Id = 1_000_000 };

        // Act
        var result = await pageModel.OnGetAsync();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
