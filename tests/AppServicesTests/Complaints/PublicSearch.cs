using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.Domain.Complaints;
using Cts.TestData;
using FluentAssertions.Execution;
using GaEpd.AppLibrary.Pagination;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class PublicSearch
{
    [Test]
    public async Task WhenItemsExist_ReturnsPagedList()
    {
        var itemList = new ReadOnlyCollection<Complaint>(ComplaintData.GetComplaints.ToList());
        var count = ComplaintData.GetComplaints.Count();
        var paging = new PaginatedRequest(1, 100);
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l =>
                l.GetPagedListAsync(It.IsAny<Expression<Func<Complaint, bool>>>(),
                    It.IsAny<PaginatedRequest>(), CancellationToken.None))
            .ReturnsAsync(itemList);
        repoMock.Setup(l =>
                l.CountAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), CancellationToken.None))
            .ReturnsAsync(count);
        var appService = new ComplaintAppService(repoMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.PublicSearchAsync(Mock.Of<ComplaintPublicSearchDto>(),
            paging, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Items.Should().BeEquivalentTo(itemList);
            result.CurrentCount.Should().Be(count);
        }
    }
    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyPagedList()
    {
        var itemList = new ReadOnlyCollection<Complaint>(new List<Complaint>());
        const int count = 0;
        var paging = new PaginatedRequest(1, 100);
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l =>
                l.GetPagedListAsync(It.IsAny<Expression<Func<Complaint, bool>>>(),
                    It.IsAny<PaginatedRequest>(), CancellationToken.None))
            .ReturnsAsync(itemList);
        repoMock.Setup(l =>
                l.CountAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), CancellationToken.None))
            .ReturnsAsync(count);
        var appService = new ComplaintAppService(repoMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.PublicSearchAsync(Mock.Of<ComplaintPublicSearchDto>(),
            paging, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Items.Should().BeEmpty();
            result.CurrentCount.Should().Be(count);
        }
    }
}
