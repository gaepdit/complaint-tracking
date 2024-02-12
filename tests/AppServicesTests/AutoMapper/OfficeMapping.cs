using Cts.AppServices.Offices;
using Cts.Domain.Entities.Offices;
using Cts.Domain.Identity;
using Cts.TestData.Constants;

namespace AppServicesTests.AutoMapper;

public class OfficeMapping
{
    [Test]
    public void OfficeViewMappingWorks()
    {
        var item = new Office(Guid.NewGuid(), TextData.ValidName);

        var result = AppServicesTestsSetup.Mapper!.Map<OfficeViewDto>(item);

        using var scope = new AssertionScope();
        result.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
        result.Active.Should().BeTrue();
    }

    [Test]
    public void OfficeAdminViewMappingWorks()
    {
        var item = new Office(Guid.NewGuid(), TextData.ValidName);

        var result = AppServicesTestsSetup.Mapper!.Map<OfficeWithAssignorDto>(item);

        using var scope = new AssertionScope();
        result.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
        result.Assignor.Should().BeNull();
        result.Active.Should().BeTrue();
    }

    [Test]
    public void OfficeUpdateMappingWorks()
    {
        var item = new Office(Guid.NewGuid(), TextData.ValidName);

        var result = AppServicesTestsSetup.Mapper!.Map<OfficeUpdateDto>(item);

        using var scope = new AssertionScope();
        result.Name.Should().Be(item.Name);
        result.Active.Should().BeTrue();
    }

    [Test]
    public void OfficeUpdateWithAdminMappingWorks()
    {
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };
        var item = new Office(Guid.NewGuid(), TextData.ValidName) { Assignor = user };

        var result = AppServicesTestsSetup.Mapper!.Map<OfficeUpdateDto>(item);

        using var scope = new AssertionScope();
        result.Name.Should().Be(item.Name);
        result.AssignorId.Should().Be(user.Id);
        result.Active.Should().BeTrue();
    }
}
