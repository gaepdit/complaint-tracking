using Cts.AppServices.ActionTypes;
using Cts.Domain.Entities.ActionTypes;

namespace AppServicesTests.AutoMapper;

public class ActionTypeMapping

{
    [Test]
    public void ActionTypeViewMappingWorks()
    {
        var item = new ActionType(Guid.NewGuid(), "Name");

        var result = AppServicesTestsSetup.Mapper!.Map<ActionTypeViewDto>(item);

        using (new AssertionScope())
        {
            result.Id.Should().Be(item.Id);
            result.Name.Should().Be(item.Name);
            result.Active.Should().BeTrue();
        }
    }

    [Test]
    public void ActionTypeUpdateMappingWorks()
    {
        var item = new ActionType(Guid.NewGuid(), "Name");

        var result = AppServicesTestsSetup.Mapper!.Map<ActionTypeUpdateDto>(item);

        using var scope = new AssertionScope();
        result.Name.Should().Be(item.Name);
        result.Active.Should().BeTrue();
    }
}
