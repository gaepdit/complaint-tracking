using Cts.AppServices.Concerns;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Concerns;
using Cts.Domain.Identity;
using Cts.TestData.Constants;

namespace AppServicesTests.Concerns;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new Concern(Guid.NewGuid(), TextData.ValidName);
        var repoMock = Substitute.For<IConcernRepository>();
        var managerMock = Substitute.For<IConcernManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns((ApplicationUser?)null);
        var appService = new ConcernService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.CreateAsync(item.Name);

        result.Should().Be(item.Id);
    }
}
