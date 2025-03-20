using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Complaints.QueryDto;

namespace WebAppTests.Pages.StaffComplaints;

public class DetailsPageGetTests
{
    private static readonly ComplaintViewDto ItemTest = new() { Id = 1 };

    [Test]
    public async Task OnGetReturnsWithCorrectPermissions()
    {
        // Arrange
        var complaintServiceMock = Substitute.For<IComplaintService>();
        complaintServiceMock.FindAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(ItemTest);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Is<IAuthorizationRequirement[]>(x => x.Contains(ComplaintOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Success());
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Is<IAuthorizationRequirement[]>(x => !x.Contains(ComplaintOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Failed());

        var page = PageModelHelpers.BuildDetailsPageModel(complaintService: complaintServiceMock,
            authorizationService: authorizationMock);
        page.Id = ItemTest.Id;
        page.TempData = WebAppTestsSetup.PageTempData();
        page.PageContext = WebAppTestsSetup.PageContextWithUser();

        // Act
        await page.OnGetAsync();

        // Assert
        using var scope = new AssertionScope();
        page.ComplaintView.Should().BeEquivalentTo(ItemTest);
        page.UserCan.Should().NotBeEmpty();
        page.UserCan[ComplaintOperation.ManageDeletions].Should().BeTrue();
        page.UserCan[ComplaintOperation.Accept].Should().BeFalse();
    }

    [Test]
    public async Task OnGetAsync_DefaultId_ReturnsRedirectToPageResult()
    {
        var page = PageModelHelpers.BuildDetailsPageModel();
        var result = await page.OnGetAsync();
        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Test]
    public async Task OnGetAsync_IdNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        const int id = 0;

        var complaintService = Substitute.For<IComplaintService>();
        complaintService.FindAsync(id).Returns((ComplaintViewDto?)null);

        var page = PageModelHelpers.BuildDetailsPageModel(complaintService: complaintService);
        page.Id = 1_000_000;

        // Act
        var result = await page.OnGetAsync();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
