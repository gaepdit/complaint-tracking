using Cts.AppServices.Complaints;
using Cts.WebApp.Pages.Public.Complaint;
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

        var service = new Mock<IComplaintAppService>();
        service.Setup(l => l.GetPublicViewAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(item);
        var pageModel = new IndexModel();

        var result = await pageModel.OnGetAsync(service.Object, 1);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.Item.Should().Be(item);
        }
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var service = new Mock<IComplaintAppService>();
        var pageModel = new IndexModel();

        var result = await pageModel.OnGetAsync(service.Object, null);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var service = new Mock<IComplaintAppService>();
        service.Setup(l => l.GetPublicViewAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync((ComplaintPublicViewDto?)null);
        var pageModel = new IndexModel();

        var result = await pageModel.OnGetAsync(service.Object, 0);

        using (new AssertionScope())
        {
            result.Should().BeOfType<NotFoundObjectResult>();
            ((NotFoundObjectResult)result).Value.Should().Be("ID not found.");
        }
    }
}
