using Cts.AppServices.ActionTypes;
using Cts.AppServices.ComplaintActions;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.Complaints.Permissions;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.WebApp.Pages.Staff.Complaints;

namespace WebAppTests.Pages.Staff.Complaints;

public class DetailsTests
{
    private static readonly ComplaintViewDto ItemTest = new() { Id = 0 };
    private static readonly StaffViewDto StaffViewTest = new() { Id = Guid.Empty.ToString(), Active = true };

    private IComplaintActionService _actionService = null!;
    private IActionTypeService _actionTypeService = null!;
    private IAuthorizationService _authorizationService = null!;
    private IStaffService _staffService = null!;

    [SetUp]
    public void SetUp()
    {
        _actionService = Substitute.For<IComplaintActionService>();
        _actionTypeService = Substitute.For<IActionTypeService>();
        _authorizationService = Substitute.For<IAuthorizationService>();
        _staffService = Substitute.For<IStaffService>();
        _staffService.GetCurrentUserAsync().Returns(StaffViewTest);
    }

    [TearDown]
    public void TearDown()
    {
        _actionService.Dispose();
        _actionTypeService.Dispose();
        _staffService.Dispose();
    }

    private DetailsModel BuildPageModel(IComplaintService complaintService) =>
        new(complaintService, _actionService, _actionTypeService, _staffService, _authorizationService);

    private DetailsModel BuildPageModel(IComplaintService complaintService, IAuthorizationService authService) =>
        new(complaintService, _actionService, _actionTypeService, _staffService, authService);


    [Test]
    public async Task OnGetReturnsWithCorrectPermissions()
    {
        // Arrange
        var complaintServiceMock = Substitute.For<IComplaintService>();
        complaintServiceMock.FindAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(ItemTest);

        var authorizationServiceMock = Substitute.For<IAuthorizationService>();
        authorizationServiceMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Is<IAuthorizationRequirement[]>(x => x.Contains(ComplaintOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Success());
        authorizationServiceMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Is<IAuthorizationRequirement[]>(x => !x.Contains(ComplaintOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Failed());

        var page = BuildPageModel(complaintServiceMock, authorizationServiceMock);
        page.TempData = WebAppTestsSetup.PageTempData();
        page.PageContext = WebAppTestsSetup.PageContextWithUser();

        // Act
        await page.OnGetAsync(ItemTest.Id);

        // Assert
        using var scope = new AssertionScope();
        page.Item.Should().BeEquivalentTo(ItemTest);
        page.UserCan.Should().NotBeEmpty();
        page.UserCan[ComplaintOperation.ManageDeletions].Should().BeTrue();
        page.UserCan[ComplaintOperation.Accept].Should().BeFalse();
    }

    [Test]
    public async Task OnGetAsync_NullId_ReturnsRedirectToPageResult()
    {
        var page = BuildPageModel(Substitute.For<IComplaintService>());
        var result = await page.OnGetAsync(null);
        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Test]
    public async Task OnGetAsync_CaseNotFound_ReturnsNotFoundResult()
    {
        const int id = 0;
        var caseworkService = Substitute.For<IComplaintService>();
        caseworkService.FindAsync(id).Returns((ComplaintViewDto?)null);
        var page = BuildPageModel(caseworkService);

        var result = await page.OnGetAsync(id);
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPostAsync_NullId_ReturnsRedirectToPageResult()
    {
        var page = BuildPageModel(Substitute.For<IComplaintService>());
        var result = await page.OnPostAsync(null);
        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Test]
    public async Task OnPostAsync_CaseNotFound_ReturnsBadRequestResult()
    {
        const int id = 0;
        var caseworkService = Substitute.For<IComplaintService>();
        caseworkService.FindAsync(id).Returns((ComplaintViewDto?)null);
        var page = BuildPageModel(caseworkService);
        page.NewAction = new ComplaintActionCreateDto(id);

        var result = await page.OnPostAsync(id);

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_MismatchedId_ReturnsBadRequest()
    {
        const int id = 0;
        var caseworkService = Substitute.For<IComplaintService>();
        var page = BuildPageModel(caseworkService);
        page.NewAction = new ComplaintActionCreateDto(999);

        var result = await page.OnPostAsync(id);

        result.Should().BeOfType<BadRequestResult>();
    }
}
