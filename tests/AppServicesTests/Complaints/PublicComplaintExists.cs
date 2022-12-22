using Cts.AppServices.Complaints;
using Cts.Domain.Complaints;
using System.Linq.Expressions;

namespace AppServicesTests.Complaints;

public class PublicComplaintExists
{
    [Test]
    public async Task WhenItemExists_ReturnsTrue()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l =>
                l.ExistsAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), CancellationToken.None))
            .ReturnsAsync(true);
        var appService = new ComplaintAppService(repoMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.PublicComplaintExistsAsync(0);

        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNoItemExists_ReturnsFalse()
    {
        var repoMock = new Mock<IComplaintRepository>();
        repoMock.Setup(l =>
                l.ExistsAsync(It.IsAny<Expression<Func<Complaint, bool>>>(), CancellationToken.None))
            .ReturnsAsync(false);
        var appService = new ComplaintAppService(repoMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.PublicComplaintExistsAsync(0);

        result.Should().BeFalse();
    }
}
