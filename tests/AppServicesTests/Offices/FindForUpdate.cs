using AutoMapper;
using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;

namespace AppServicesTests.Offices;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            GivenName = TestConstants.ValidName,
            FamilyName = TestConstants.NewValidName,
            Email = TestConstants.ValidEmail,
        };
        var office = new Office(Guid.Empty, TestConstants.ValidName) { Assignor = user };

        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindIncludeAssignorAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(office);
        var managerMock = new Mock<IOfficeManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsSetup.Mapper!, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(office);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var managerMock = new Mock<IOfficeManager>();
        var mapperMock = new Mock<IMapper>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            mapperMock.Object, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
