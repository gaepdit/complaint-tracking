using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.WebApp.Pages.Public.Complaints;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppTests.Pages.Public.Complaint;

public class IndexTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var item = Mock.Of<ComplaintPublicViewDto>();

        var serviceMock = new Mock<IComplaintAppService>();
        serviceMock.Setup(l => l.GetPublicAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(item);
        var pageModel = new IndexModel();

        var result = await pageModel.OnGetAsync(serviceMock.Object, 1);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.Item.Should().Be(item);
        }
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var serviceMock = new Mock<IComplaintAppService>();
        var pageModel = new IndexModel();

        var result = await pageModel.OnGetAsync(serviceMock.Object, null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("../Index");
        }
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var serviceMock = new Mock<IComplaintAppService>();
        serviceMock.Setup(l => l.GetPublicAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync((ComplaintPublicViewDto?)null);
        var pageModel = new IndexModel();

        var result = await pageModel.OnGetAsync(serviceMock.Object, 0);

        result.Should().BeOfType<NotFoundResult>();
    }
}
