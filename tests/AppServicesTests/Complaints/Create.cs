using Cts.AppServices.Complaints;
using Cts.AppServices.Complaints.Dto;
using Cts.AppServices.UserServices;
using Cts.Domain.Complaints;
using Cts.Domain.Concerns;
using Cts.Domain.Offices;

namespace AppServicesTests.Complaints;

public class Create
{
    [Test]
    public async Task OnSuccessfulInsert_ReturnsId()
    {
        var item = new ComplaintCreateDto();

        var appService = new ComplaintAppService(
            Mock.Of<IComplaintRepository>(),
            Mock.Of<IConcernRepository>(),
            Mock.Of<IOfficeRepository>(),
            Mock.Of<IComplaintManager>(),
            AppServicesTestsGlobal.Mapper!,
            Mock.Of<IUserService>());

        var result = await appService.CreateAsync(item);

        result.Should().Be(0);
    }
}
