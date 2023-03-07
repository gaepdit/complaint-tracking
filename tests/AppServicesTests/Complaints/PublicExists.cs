﻿using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Complaints;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class PublicExists
{
    [Test]
    public async Task WhenItemExists_ReturnsTrue()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l =>
                l.ExistsAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), CancellationToken.None))
            .ReturnsAsync(true);
        var appService = new ComplaintAppService(repoMock.Object, Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!, Mock.Of<IUserService>());

        var result = await appService.PublicExistsAsync(0);

        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsFalse()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l =>
                l.ExistsAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), CancellationToken.None))
            .ReturnsAsync(false);
        var appService = new ComplaintAppService(repoMock.Object, Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!, Mock.Of<IUserService>());

        var result = await appService.PublicExistsAsync(0);

        result.Should().BeFalse();
    }
}