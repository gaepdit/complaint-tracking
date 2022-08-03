using AutoMapper;
using Cts.Application.ActionTypes;
using Cts.Application.Offices;
using Cts.Domain.ActionTypes;

namespace ApplicationTests.AutoMapper;

public class AutoMapperTests
{
    [Test]
    public void ActionTypeMappingWorks()
    {
        var id = Guid.NewGuid();
        const string name = "Name";
        var actionType = new ActionType(id, name);

        var result = Global.Mapper!.Map<ActionTypeViewDto>(actionType);

        result.Id.Should().Be(id);
        result.Name.Should().Be(name);
        result.Active.Should().BeTrue();
    }

    [Test]
    public void MappingConfigurationsAreValid()
    {
        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile(new ActionTypeMappingProfile());
            c.AddProfile(new OfficeMappingProfile());
        });
        mapperConfig.AssertConfigurationIsValid();
    }
}
