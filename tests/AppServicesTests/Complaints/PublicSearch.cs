using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.QueryDto;
using Cts.AppServices.Notifications;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.TestData;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class PublicSearch
{
    [Test]
    public async Task WhenItemsExist_ReturnsPagedList()
    {
        // Arrange
        var itemList = new ReadOnlyCollection<Complaint>(ComplaintData.GetComplaints.ToList());
        var count = ComplaintData.GetComplaints.Count();
        var paging = new PaginatedRequest(1, 100);

        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<Complaint, bool>>>(),
                Arg.Any<PaginatedRequest>())
            .Returns(itemList);
        repoMock.CountAsync(Arg.Any<Expression<Func<Complaint, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), Substitute.For<INotificationService>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>(),
            Substitute.For<ILogger<ComplaintService>>());

        // Act
        var result = await appService.PublicSearchAsync(new ComplaintPublicSearchDto(), paging, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEquivalentTo(itemList);
        result.CurrentCount.Should().Be(count);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyPagedList()
    {
        // Arrange
        var itemList = new ReadOnlyCollection<Complaint>(new List<Complaint>());
        const int count = 0;
        var paging = new PaginatedRequest(1, 100);

        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<Complaint, bool>>>(),
                Arg.Any<PaginatedRequest>())
            .Returns(itemList);
        repoMock.CountAsync(Arg.Any<Expression<Func<Complaint, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), Substitute.For<INotificationService>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>(),
            Substitute.For<ILogger<ComplaintService>>());

        // Act
        var result = await appService.PublicSearchAsync(new ComplaintPublicSearchDto(), paging, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEmpty();
        result.CurrentCount.Should().Be(count);
    }
}
