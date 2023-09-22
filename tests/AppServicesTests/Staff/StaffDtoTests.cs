using Cts.AppServices.Offices;
using Cts.AppServices.Staff.Dto;
using Cts.TestData.Constants;

namespace AppServicesTests.Staff;

public class StaffDtoTests
{
    [Test]
    public void DisplayName_TrimAll_TrimsItems()
    {
        var staffSearchDto = new StaffSearchDto(SortBy.NameAsc, " abc ", " def ", null, null, null);

        var result = staffSearchDto.TrimAll();

        using (new AssertionScope())
        {
            result.Name.Should().Be("abc");
            result.Email.Should().Be("def");
        }
    }

    private static StaffViewDto ValidStaffView => new()
    {
        Id = Guid.Empty.ToString(),
        FamilyName = string.Empty,
        GivenName = string.Empty,
    };

    [TestCase("abc", "def", "abc def")]
    [TestCase("abc", "", "abc")]
    [TestCase("", "def", "def")]
    public void DisplayName_ExpectedBehavior(string givenName, string familyName, string expected)
    {
        var staffViewDto = ValidStaffView with { GivenName = givenName, FamilyName = familyName };
        staffViewDto.Name.Should().Be(expected);
    }

    [TestCase("abc", "def", "def, abc")]
    [TestCase("abc", "", "abc")]
    [TestCase("", "def", "def")]
    public void SortableFullName_ExpectedBehavior(string givenName, string familyName, string expected)
    {
        var staffViewDto = ValidStaffView with { GivenName = givenName, FamilyName = familyName };
        staffViewDto.SortableFullName.Should().Be(expected);
    }

    [Test]
    public void AsUpdateDto_ExpectedBehavior()
    {
        var staffViewDto = ValidStaffView with
        {
            Id = Guid.NewGuid().ToString(),
            Active = true,
            Phone = TextData.ValidPhoneNumber,
            Office = new OfficeViewDto(Guid.NewGuid(), TextData.ValidName, true),
        };

        var result = staffViewDto.AsUpdateDto();

        using (new AssertionScope())
        {
            result.Id.Should().Be(staffViewDto.Id);
            result.Active.Should().BeTrue();
            result.Phone.Should().Be(staffViewDto.Phone);
            result.OfficeId.Should().Be(staffViewDto.Office.Id);
        }
    }
}
