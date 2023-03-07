using Cts.AppServices.Offices;
using Cts.Domain.Identity;
using Cts.Domain.Offices;
using Cts.TestData.Constants;
using FluentAssertions.Execution;

namespace AppServicesTests.AutoMapper;

public class OfficeMapping
{
    [Test]
    public void OfficeViewMappingWorks()
    {
        var item = new Office(Guid.NewGuid(), TestConstants.ValidName);

        var result = AppServicesTestsGlobal.Mapper!.Map<OfficeAdminViewDto>(item);

        using (new AssertionScope())
        {
            result.Id.Should().Be(item.Id);
            result.Name.Should().Be(item.Name);
            result.Assignor.Should().BeNull();
            result.Active.Should().BeTrue();
        }
    }

    [Test]
    public void OfficeUpdateMappingWorks()
    {
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };
        var item = new Office(Guid.NewGuid(), TestConstants.ValidName) { AssignorId = user.Id };

        var result = AppServicesTestsGlobal.Mapper!.Map<OfficeUpdateDto>(item);

        using (new AssertionScope())
        {
            result.Id.Should().Be(item.Id);
            result.Name.Should().Be(item.Name);
            result.AssignorId.Should().Be(user.Id);
            result.Active.Should().BeTrue();
        }
    }
}
