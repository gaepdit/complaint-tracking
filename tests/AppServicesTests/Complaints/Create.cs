using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Complaints;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Entities.Offices;
using Cts.TestData.Constants;

namespace AppServicesTests.Complaints;

public class Create
{
    [Test]
    public async Task OnSuccessfulInsert_ReturnsId()
    {
        var repoMock = new Mock<IOfficeRepository>();
        var office = new Office(Guid.NewGuid(), TestConstants.ValidName);
        repoMock.Setup(l => l.GetAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(office);
        var item = new ComplaintCreateDto() { CurrentOfficeId = Guid.Empty};

        var appService = new ComplaintAppService(
            Mock.Of<IComplaintRepository>(),
            Mock.Of<IConcernRepository>(),
            repoMock.Object,
            Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!,
            Mock.Of<IUserService>());

        var result = await appService.CreateAsync(item);

        result.Should().Be(0);
    }
}
