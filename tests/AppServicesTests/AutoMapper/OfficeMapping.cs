using Cts.AppServices.Offices;
using Cts.Domain.Entities;

namespace AppServicesTests.AutoMapper;

public class OfficeMapping
{
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
    public void OfficeUpdateMappingWorks()
    {
        var item = new Office(Guid.NewGuid(), "Name");

        var result = AppServicesTestsGlobal.Mapper!.Map<OfficeUpdateDto>(item);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(item.Id);
            result.Name.Should().Be(item.Name);
            result.MasterUser.Should().BeNull();
            result.Active.Should().BeTrue();
        });
    }
}
