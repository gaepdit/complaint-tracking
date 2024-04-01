using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Microsoft.AspNetCore.Authorization;

namespace AppServicesTests.Complaints;

public class Exists
{
    [Test]
    public async Task WhenItemExists_ReturnsTrue()
    {
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.ExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(true);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>(),
            Substitute.For<IAuthorizationService>());

        var result = await appService.ExistsAsync(0);

        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsFalse()
    {
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.ExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>(),
            Substitute.For<IAuthorizationService>());

        var result = await appService.ExistsAsync(0);

        result.Should().BeFalse();
    }
}
