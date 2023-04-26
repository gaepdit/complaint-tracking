using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.TestData.Identity;

namespace AppServicesTests.Staff;

public class StaffFilters
{
    [Test]
    public void DefaultFilter_ReturnsAllActive()
    {
        var spec = new StaffSearchDto();
        var expected = UserData.GetUsers.Where(e => e.Active);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NameFilter_ReturnsMatches()
    {
        var name = UserData.GetUsers.First(e => e.Active).GivenName;
        var spec = new StaffSearchDto { Name = name };
        var expected = UserData.GetUsers
            .Where(e => e.Active &&
                (string.Equals(e.GivenName, name, StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(e.FamilyName, name, StringComparison.CurrentCultureIgnoreCase)));

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EmailFilter_ReturnsMatches()
    {
        var email = UserData.GetUsers.First(e => e.Active).Email;
        var spec = new StaffSearchDto { Email = email };
        var expected = UserData.GetUsers
            .Where(e => e.Active && e.Email == email);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void OfficeFilter_ReturnsMatches()
    {
        var office = UserData.GetUsers.First(e => e is { Active: true, Office: { } }).Office;
        var spec = new StaffSearchDto { Office = office!.Id };
        var expected = UserData.GetUsers
            .Where(e => e.Active && e.Office == office);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void InactiveFilter_ReturnsAllInactive()
    {
        var spec = new StaffSearchDto { Status = SearchStaffStatus.Inactive };
        var expected = UserData.GetUsers.Where(e => !e.Active);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void StandaloneActiveStatusFilter_ReturnsAllActive()
    {
        var expected = UserData.GetUsers.Where(e => e.Active);

        var result = UserData.GetUsers.AsQueryable().FilterByActiveStatus(SearchStaffStatus.Active);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void StandaloneActiveStatusFilterForInactive_ReturnsAllActive()
    {
        var expected = UserData.GetUsers.Where(e => !e.Active);

        var result = UserData.GetUsers.AsQueryable().FilterByActiveStatus(SearchStaffStatus.Inactive);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void StatusAllFilter_ReturnsAll()
    {
        var spec = new StaffSearchDto { Status = SearchStaffStatus.All };

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(UserData.GetUsers);
    }
}
