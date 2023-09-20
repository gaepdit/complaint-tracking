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
            GivenName = TextData.ValidName,
            FamilyName = TextData.NewValidName,
            Email = TextData.ValidEmail,
        };
        var office = new Office(Guid.Empty, TextData.ValidName) { Assignor = user };

        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindIncludeAssignorAsync(office.Id, Arg.Any<CancellationToken>()).Returns(office);
        var managerMock = Substitute.For<IOfficeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new OfficeService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(office);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>()).Returns((Office?)null);
        var managerMock = Substitute.For<IOfficeManager>();
        var mapperMock = Substitute.For<IMapper>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new OfficeService(repoMock, managerMock,
            mapperMock, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
