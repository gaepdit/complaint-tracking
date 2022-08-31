using Cts.AppServices.Offices;
using Cts.AppServices.StaffServices;
using Cts.Domain.Entities;

namespace AppServicesTests.AutoMapper;

public class UserMapping
{
    [Test]
    public void StaffViewMappingWorks()
    {
        var item = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Local",
            LastName = "User",
            Email = "local.user@example.net",
            Phone = "123-456-780",
            Office = new Office(Guid.NewGuid(), "Office"),
        };

        var result = AppServicesTestsGlobal.Mapper!.Map<StaffViewDto>(item);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(item.Id);
            result.FirstName.Should().Be(item.FirstName);
            result.LastName.Should().Be(item.LastName);
            result.Email.Should().Be(item.Email);
            result.Phone.Should().Be(item.Phone);
            result.Office.Should().BeEquivalentTo(item.Office);
            result.Active.Should().BeTrue();
        });
    }

    [Test]
    public void StaffViewReverseMappingWorks()
    {
        var item = new StaffViewDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Local",
            LastName = "User",
            Email = "local.user@example.net",
            Phone = "123-456-780",
            Office = new OfficeViewDto { Id = Guid.NewGuid(), Name = "Office" },
        };

        var result = AppServicesTestsGlobal.Mapper!.Map<ApplicationUser>(item);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(item.Id.ToString());
            result.FirstName.Should().Be(item.FirstName);
            result.LastName.Should().Be(item.LastName);
            result.Email.Should().Be(item.Email);
            result.Phone.Should().Be(item.Phone);
            result.Office.Should().BeEquivalentTo(item.Office);
            result.Active.Should().BeTrue();
        });
    }

    [Test]
    public void StaffUpdateMappingWorks()
    {
        var item = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Local",
            LastName = "User",
            Email = "local.user@example.net",
            Phone = "123-456-780",
            Office = new Office(Guid.NewGuid(), "Office"),
        };

        var result = AppServicesTestsGlobal.Mapper!.Map<StaffUpdateDto>(item);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(item.Id);
            result.Phone.Should().Be(item.Phone);
            result.OfficeId.Should().Be(item.Office.Id);
            result.Active.Should().BeTrue();
        });
    }

    [Test]
    public void NullStaffViewMappingWorks()
    {
        ApplicationUser? item = null;
        var result = AppServicesTestsGlobal.Mapper!.Map<StaffViewDto?>(item);
        result.Should().BeNull();
    }
}
