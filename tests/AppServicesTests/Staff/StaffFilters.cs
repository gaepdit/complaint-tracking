using MyAppRoot.AppServices.Staff;
using MyAppRoot.TestData.Identity;

namespace AppServicesTests.Staff;

public class StaffFilters
{
    [Test]
    public void DefaultFilter_ReturnsAllActive()
    {
        var filter = new StaffSearchDto();
        var expected = IdentityData.GetUsers.Where(e => e.Active);

        var result = IdentityData.GetUsers.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NameFilter_ReturnsMatches()
    {
        var name = IdentityData.GetUsers.First(e => e.Active).FirstName;
        var filter = new StaffSearchDto { Name = name };
        var expected = IdentityData.GetUsers
            .Where(e => e.Active && (e.FirstName == name || e.LastName == name));

        var result = IdentityData.GetUsers.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EmailFilter_ReturnsMatches()
    {
        var email = IdentityData.GetUsers.First(e => e.Active).Email;
        var filter = new StaffSearchDto { Email = email };
        var expected = IdentityData.GetUsers
            .Where(e => e.Active && e.Email == email);

        var result = IdentityData.GetUsers.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void OfficeFilter_ReturnsMatches()
    {
        var office = IdentityData.GetUsers.First(e => e.Active && e.Office != null).Office;
        var filter = new StaffSearchDto { Office = office!.Id };
        var expected = IdentityData.GetUsers
            .Where(e => e.Active && e.Office == office);

        var result = IdentityData.GetUsers.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void InactiveFilter_ReturnsAllInactive()
    {
        var filter = new StaffSearchDto { Status = StaffSearchDto.ActiveStatus.Inactive };
        var expected = IdentityData.GetUsers.Where(e => !e.Active);

        var result = IdentityData.GetUsers.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void StatusAllFilter_ReturnsAll()
    {
        var filter = new StaffSearchDto { Status = StaffSearchDto.ActiveStatus.All };
        var result = IdentityData.GetUsers.AsQueryable().ApplyFilter(filter);
        result.Should().BeEquivalentTo(IdentityData.GetUsers);
    }
}
