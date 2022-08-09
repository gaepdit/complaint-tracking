using Cts.AppServices.ActionTypes;
using Cts.AppServices.Offices;
using Cts.AppServices.Users;
using Cts.Domain.Entities;
using Cts.Domain.Users;

namespace AppServicesTests.AutoMapper;

public class AutoMapperTests
{
    [Test]
    public void MappingConfigurationsAreValid()
    {
        AppServicesTestsGlobal.MapperConfig!.AssertConfigurationIsValid();
    }

    [Test]
    public void ActionTypeViewMappingWorks()
    {
        var item = new ActionType(Guid.NewGuid(), "Name");

        var result = AppServicesTestsGlobal.Mapper!.Map<ActionTypeViewDto>(item);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(item.Id);
            result.Name.Should().Be(item.Name);
            result.Active.Should().BeTrue();
        });
    }

    [Test]
    public void OfficeViewMappingWorks()
    {
        var item = new Office(Guid.NewGuid(), "Name");

        var result = AppServicesTestsGlobal.Mapper!.Map<OfficeViewDto>(item);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(item.Id);
            result.Name.Should().Be(item.Name);
            result.MasterUser.Should().BeNull();
            result.Active.Should().BeTrue();
        });
    }

    [Test]
    public void UserViewMappingWorks()
    {
        var item = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Local",
            LastName = "User",
            Email = "local.user@example.net",
        };

        var result = AppServicesTestsGlobal.Mapper!.Map<UserViewDto>(item);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(item.Id);
            result.FirstName.Should().Be(item.FirstName);
            result.LastName.Should().Be(item.LastName);
            result.Email.Should().Be(item.Email);
            result.Active.Should().BeTrue();
            result.Office.Should().BeNull();
        });
    }

    [Test]
    public void NullUserMappingWorks()
    {
        ApplicationUser? item = null;
        var result = AppServicesTestsGlobal.Mapper!.Map<UserViewDto?>(item);
        result.Should().BeNull();
    }
}
