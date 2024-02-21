using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.WebApp.Pages.Public.Complaints;

namespace WebAppTests.Pages.Public.Complaint;

public class IndexPageTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var item = new ComplaintPublicViewDto();

        var serviceMock = Substitute.For<IComplaintService>();
        serviceMock.FindPublicAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var pageModel = new IndexModel();

        var result = await pageModel.OnGetAsync(serviceMock, 1);

        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        pageModel.Item.Should().Be(item);
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var serviceMock = Substitute.For<IComplaintService>();
        var pageModel = new IndexModel();

        var result = await pageModel.OnGetAsync(serviceMock, null);

        using var scope = new AssertionScope();
        result.Should().BeOfType<RedirectToPageResult>();
        ((RedirectToPageResult)result).PageName.Should().Be("../Index");
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var serviceMock = Substitute.For<IComplaintService>();
        serviceMock.FindPublicAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns((ComplaintPublicViewDto?)null);
        var pageModel = new IndexModel();

        var result = await pageModel.OnGetAsync(serviceMock, 0);

        result.Should().BeOfType<NotFoundResult>();
    }
}
