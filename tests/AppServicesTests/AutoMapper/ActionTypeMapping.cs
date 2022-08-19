using Cts.AppServices.ActionTypes;
using Cts.Domain.Entities;

namespace AppServicesTests.AutoMapper;

public class ActionTypeMapping

{
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
    public void ActionTypeUpdateMappingWorks()
    {
        var item = new ActionType(Guid.NewGuid(), "Name");

        var result = AppServicesTestsGlobal.Mapper!.Map<ActionTypeUpdateDto>(item);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(item.Id);
            result.Name.Should().Be(item.Name);
            result.Active.Should().BeTrue();
        });
    }
}
