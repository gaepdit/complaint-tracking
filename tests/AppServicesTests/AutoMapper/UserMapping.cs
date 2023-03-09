using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.Domain.Identity;
using Cts.Domain.Offices;
using Cts.TestData.Constants;
using FluentAssertions.Execution;

namespace AppServicesTests.AutoMapper;

public class UserMapping
{
    private readonly ApplicationUser _item = new()
    {
        Id = Guid.NewGuid().ToString(),
        GivenName = TestConstants.ValidName,
        FamilyName = TestConstants.ValidName,
        Email = TestConstants.ValidEmail,
        Phone = "123-456-7890",
        Office = new Office(Guid.NewGuid(), TestConstants.ValidName),
    };

    [Test]
    public void StaffViewMappingWorks()
    {
        var result = AppServicesTestsGlobal.Mapper!.Map<StaffViewDto>(_item);

        using (new AssertionScope())
        {
            result.Id.Should().Be(_item.Id);
            result.GivenName.Should().Be(_item.GivenName);
            result.FamilyName.Should().Be(_item.FamilyName);
            result.Email.Should().Be(_item.Email);
            result.Phone.Should().Be(_item.Phone);
            result.Office.Should().BeEquivalentTo(_item.Office);
            result.Active.Should().BeTrue();
        }
    }

    //[Test]
    //public void StaffViewReverseMappingWorks()
    //{
    //    var item = new StaffViewDto
    //    {
    //        Id = Guid.NewGuid().ToString(),
    //        GivenName = TestConstants.ValidName,
    //        FamilyName = TestConstants.ValidName,
    //        Email = TestConstants.ValidEmail,
    //        Phone = "123-456-7890",
    //        Office = new OfficeDisplayViewDto { Id = Guid.NewGuid(), Name = TestConstants.ValidName },
    //    };

    //    var result = AppServicesTestsGlobal.Mapper!.Map<ApplicationUser>(item);

    //    using (new AssertionScope())
    //    {
    //        result.Id.Should().Be(item.Id);
    //        result.GivenName.Should().Be(item.GivenName);
    //        result.FamilyName.Should().Be(item.FamilyName);
    //        result.Email.Should().Be(item.Email);
    //        result.Phone.Should().Be(item.Phone);
    //        result.Office.Should().BeEquivalentTo(item.Office);
    //        result.Active.Should().BeTrue();
    //    }
    //}

    [Test]
    public void StaffUpdateMappingWorks()
    {
        var result = AppServicesTestsGlobal.Mapper!.Map<StaffUpdateDto>(_item);

        using (new AssertionScope())
        {
            result.Id.Should().Be(_item.Id);
            result.Phone.Should().Be(_item.Phone);
            result.OfficeId.Should().Be(_item.Office!.Id);
            result.Active.Should().BeTrue();
        }
    }

    [Test]
    public void NullStaffViewMappingWorks()
    {
        ApplicationUser? item = null;
        var result = AppServicesTestsGlobal.Mapper!.Map<StaffViewDto?>(item);
        result.Should().BeNull();
    }
}
