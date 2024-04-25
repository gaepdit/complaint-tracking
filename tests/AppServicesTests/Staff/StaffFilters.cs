using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.TestData.Identity;

namespace AppServicesTests.Staff;

public class StaffFilters
{
    private static StaffSearchDto DefaultStaffSearch => new(SortBy.NameAsc, null, null, null, null, null);

    [Test]
    public void DefaultFilter_ReturnsAllActive()
    {
        var expected = UserData.GetUsers.Where(e => e.Active);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(DefaultStaffSearch);

        result.Should().BeEquivalentTo(expected, options => options.Excluding(user => user.ObjectIdentifier));
    }

    [Test]
    public void NameFilter_ReturnsMatches()
    {
        var name = UserData.GetUsers.First(e => e.Active).GivenName;
        var spec = DefaultStaffSearch with { Name = name };
        var expected = UserData.GetUsers
            .Where(e => e.Active &&
                        (string.Equals(e.GivenName, name, StringComparison.CurrentCultureIgnoreCase) ||
                         string.Equals(e.FamilyName, name, StringComparison.CurrentCultureIgnoreCase)));

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected, options => options.Excluding(user => user.ObjectIdentifier));
    }

    [Test]
    public void EmailFilter_ReturnsMatches()
    {
        var email = UserData.GetUsers.First(e => e.Active).Email;
        var spec = DefaultStaffSearch with { Email = email };
        var expected = UserData.GetUsers
            .Where(e => e.Active && e.Email == email);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected, options => options.Excluding(user => user.ObjectIdentifier));
    }

    [Test]
    public void OfficeFilter_ReturnsMatches()
    {
        var office = UserData.GetUsers.First().Office;
        var spec = DefaultStaffSearch with { Office = office!.Id, Status = SearchStaffStatus.All };
        var expected = UserData.GetUsers
            .Where(e => e.Office!.Id == office.Id);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected, options => options.Excluding(user => user.ObjectIdentifier));
    }

    [Test]
    public void InactiveFilter_ReturnsAllInactive()
    {
        var spec = DefaultStaffSearch with { Status = SearchStaffStatus.Inactive };
        var expected = UserData.GetUsers.Where(e => !e.Active);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(spec);

        result.Should().BeEquivalentTo(expected, options => options.Excluding(user => user.ObjectIdentifier));
    }

    [Test]
    public void StatusAllFilter_ReturnsAll()
    {
        var spec = DefaultStaffSearch with { Status = SearchStaffStatus.All };
        var result = UserData.GetUsers.AsQueryable().ApplyFilter(spec);
        result.Should().BeEquivalentTo(UserData.GetUsers, options => options.Excluding(user => user.ObjectIdentifier));
    }
}
