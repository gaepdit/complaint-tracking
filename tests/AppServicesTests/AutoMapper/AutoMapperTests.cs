using Cts.AppServices.ActionTypes;
using Cts.AppServices.Offices;
using Cts.Domain.ActionTypes;
using Cts.Domain.Offices;

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
        var id = Guid.NewGuid();
        const string name = "Name";
        var actionType = new ActionType(id, name);

        var result = AppServicesTestsGlobal.Mapper!.Map<ActionTypeViewDto>(actionType);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(id);
            result.Name.Should().Be(name);
            result.Active.Should().BeTrue();
        });
    }

    [Test]
    public void OfficeViewMappingWorks()
    {
        var id = Guid.NewGuid();
        const string name = "Name";
        var office = new Office(id, name);

        var result = AppServicesTestsGlobal.Mapper!.Map<OfficeViewDto>(office);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(id);
            result.Name.Should().Be(name);
            result.MasterUser.Should().BeNull();
            result.Active.Should().BeTrue();
        });
    }
}
