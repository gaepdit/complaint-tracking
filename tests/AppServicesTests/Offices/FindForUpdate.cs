using AutoMapper;
using Cts.AppServices.Offices;
using Cts.AppServices.UserServices;
using Cts.Domain.Entities;
using Cts.Domain.Offices;
using Cts.TestData.Constants;

namespace AppServicesTests.Offices;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var office = new Office(Guid.Empty, TestConstants.ValidName);
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Local",
            LastName = "User",
            Email = "local.user@example.net",
            UserName = "local.user@example.net",
            NormalizedUserName = "local.user@example.net".ToUpperInvariant(),
        };
        office.MasterUser = user;

        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindAsync(office.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(office);
        var managerMock = new Mock<IOfficeManager>();
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        Assert.Multiple(() =>
        {
            result.Should().BeEquivalentTo(office, options => options
                .Excluding(e => e.MasterUser.Id));
            result!.MasterUser!.Id.ToString().Should().Be(office.MasterUser.Id);
        });
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
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            mapperMock.Object, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
