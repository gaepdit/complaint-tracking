using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.TestData;
using GaEpd.AppLibrary.Pagination;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class Search
{
    [Test]
    public async Task WhenItemsExist_ReturnsPagedList()
    {
        var itemList = new ReadOnlyCollection<Complaint>(ComplaintData.GetComplaints.ToList());
        var count = ComplaintData.GetComplaints.Count();
        var paging = new PaginatedRequest(1, 100);
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<Complaint, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        repoMock.CountAsync(Arg.Any<Expression<Func<Complaint, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.SearchAsync(Substitute.For<ComplaintSearchDto>(),
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
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<Complaint, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        repoMock.CountAsync(
                Arg.Any<Expression<Func<Complaint, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.SearchAsync(Substitute.For<ComplaintSearchDto>(),
            paging, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Items.Should().BeEmpty();
            result.CurrentCount.Should().Be(count);
        }
    }
}
