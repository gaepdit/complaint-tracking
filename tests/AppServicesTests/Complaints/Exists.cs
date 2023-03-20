﻿using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Complaints;
using Cts.Domain.Concerns;
using Cts.Domain.Offices;

namespace AppServicesTests.Complaints;

public class Exists
{
    [Test]
    public async Task WhenItemExists_ReturnsTrue()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.ExistsAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(true);
        var appService = new ComplaintAppService(
            repoMock.Object,
            Mock.Of<IConcernRepository>(),
            Mock.Of<IOfficeRepository>(),
            Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!,
            Mock.Of<IUserService>());

        var result = await appService.ExistsAsync(0);

        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsFalse()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l => l.ExistsAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(false);
        var appService = new ComplaintAppService(
            repoMock.Object,
            Mock.Of<IConcernRepository>(),
            Mock.Of<IOfficeRepository>(),
            Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!,
            Mock.Of<IUserService>());

        var result = await appService.ExistsAsync(0);

        result.Should().BeFalse();
    }
}
