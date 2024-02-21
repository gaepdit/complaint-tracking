﻿using Cts.AppServices.Attachments;
using Cts.AppServices.Complaints;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class FindPublic
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new Complaint(1);

        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<Expression<Func<Complaint, bool>>>(), Arg.Any<bool>(),
                Arg.Any<CancellationToken>())
            .Returns(item);

        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        // Act
        var result = await appService.FindPublicAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        // Arrange
        var repoMock = Substitute.For<IComplaintRepository>();
        repoMock.FindAsync(Arg.Any<Expression<Func<Complaint, bool>>>(), Arg.Any<CancellationToken>())
            .Returns((Complaint?)null);
        var appService = new ComplaintService(repoMock, Substitute.For<IComplaintManager>(),
            Substitute.For<IConcernRepository>(), Substitute.For<IOfficeRepository>(),
            Substitute.For<IAttachmentService>(), AppServicesTestsSetup.Mapper!, Substitute.For<IUserService>());

        // Act
        var result = await appService.FindPublicAsync(0);

        // Assert
        result.Should().BeNull();
    }
}
