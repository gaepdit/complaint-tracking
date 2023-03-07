using Cts.AppServices.Staff;
using Cts.TestData.Identity;

namespace AppServicesTests.Staff;

public class StaffFilters
{
    [Test]
    public void DefaultFilter_ReturnsAllActive()
    {
        var users = UserData.GetUsers;
        var filter = new StaffSearchDto();
        var expected = users.Where(e => e.Active);

        var result = users.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NameFilter_ReturnsMatches()
    {
        var users = UserData.GetUsers;
        var name = users.First(e => e.Active).GivenName;
        var filter = new StaffSearchDto { Name = name };
        var expected = users
            .Where(e => e.Active &&
                (string.Equals(e.GivenName, name, StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(e.FamilyName, name, StringComparison.CurrentCultureIgnoreCase)));

        var result = users.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EmailFilter_ReturnsMatches()
    {
        var users = UserData.GetUsers;
        var email = users.First(e => e.Active).Email;
        var filter = new StaffSearchDto { Email = email };
        var expected = users
            .Where(e => e.Active && e.Email == email);

        var result = users.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void OfficeFilter_ReturnsMatches()
    {
        var users = UserData.GetUsers;
        var office = users.First(e => e is { Active: true, Office: { } }).Office;
        var filter = new StaffSearchDto { Office = office!.Id };
        var expected = users
            .Where(e => e.Active && e.Office == office);

        var result = users.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void InactiveFilter_ReturnsAllInactive()
    {
        var users = UserData.GetUsers;
        var filter = new StaffSearchDto { Status = StaffSearchDto.ActiveStatus.Inactive };
        var expected = users.Where(e => !e.Active);

        var result = users.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void StandaloneActiveStatusFilter_ReturnsAllActive()
    {
        var users = UserData.GetUsers;
        var expected = users.Where(e => e.Active);
        
        var result = users.AsQueryable().FilterByActiveStatus(StaffSearchDto.ActiveStatus.Active);
        
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void StandaloneActiveStatusFilterForInactive_ReturnsAllActive()
    {
        var users = UserData.GetUsers;
        var expected = users.Where(e => !e.Active);
        
        var result = users.AsQueryable().FilterByActiveStatus(StaffSearchDto.ActiveStatus.Inactive);
        
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void StatusAllFilter_ReturnsAll()
    {
        var  users = UserData.GetUsers;
        var filter = new StaffSearchDto { Status = StaffSearchDto.ActiveStatus.All };
        
        var result = users.AsQueryable().ApplyFilter(filter);
        
        result.Should().BeEquivalentTo(users);
    }
}
