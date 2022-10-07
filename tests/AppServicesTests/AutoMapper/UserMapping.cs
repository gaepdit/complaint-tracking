using MyAppRoot.AppServices.Offices;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.Domain.Identity;
using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Constants;

namespace AppServicesTests.AutoMapper;

public class UserMapping
{
    private readonly ApplicationUser _item = new()
    {
        Id = Guid.NewGuid().ToString(),
        FirstName = TestConstants.ValidName,
        LastName = TestConstants.ValidName,
        Email = TestConstants.ValidEmail,
        Phone = "123-456-7890",
        Office = new Office(Guid.NewGuid(), TestConstants.ValidName),
    };

    [Test]
    public void StaffViewMappingWorks()
    {
        var result = AppServicesTestsGlobal.Mapper!.Map<StaffViewDto>(_item);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(_item.Id);
            result.FirstName.Should().Be(_item.FirstName);
            result.LastName.Should().Be(_item.LastName);
            result.Email.Should().Be(_item.Email);
            result.Phone.Should().Be(_item.Phone);
            result.Office.Should().BeEquivalentTo(_item.Office);
            result.Active.Should().BeTrue();
        });
    }

    [Test]
    public void StaffViewReverseMappingWorks()
    {
        var item = new StaffViewDto
        {
            Id = Guid.NewGuid(),
            FirstName = TestConstants.ValidName,
            LastName = TestConstants.ValidName,
            Email = TestConstants.ValidEmail,
            Phone = "123-456-7890",
            Office = new OfficeViewDto { Id = Guid.NewGuid(), Name = TestConstants.ValidName },
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
        var result = AppServicesTestsGlobal.Mapper!.Map<StaffUpdateDto>(_item);

        Assert.Multiple(() =>
        {
            result.Id.Should().Be(_item.Id);
            result.Phone.Should().Be(_item.Phone);
            result.OfficeId.Should().Be(_item.Office!.Id);
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
