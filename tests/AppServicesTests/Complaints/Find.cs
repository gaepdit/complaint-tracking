using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.ComplaintTransitions;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;

namespace AppServicesTests.Complaints;

public class Find
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new Complaint(1);

        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(item);

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        // Act
        var result = await appService.FindAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNonPublicItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new Complaint(1);
        item.SetDeleted(null);

        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(item);

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        // Act
        var result = await appService.FindAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns((Complaint?)null);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IComplaintTransitionManager>(),
            AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        var result = await appService.FindAsync(0);

        result.Should().BeNull();
    }
}
