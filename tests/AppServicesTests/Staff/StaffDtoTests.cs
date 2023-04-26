using Cts.AppServices.Offices;
using Cts.AppServices.Staff;
using Cts.AppServices.Staff.Dto;
using Cts.TestData.Constants;
using FluentAssertions.Execution;

namespace AppServicesTests.Staff;

public class StaffDtoTests
{
    [Test]
    public void DisplayName_ExpectedBehavior()
    {
        var staffSearchDto = new StaffSearchDto { Name = " abc ", Email = " def " };

        staffSearchDto.TrimAll();

        using (new AssertionScope())
        {
            staffSearchDto.Name.Should().Be("abc");
            staffSearchDto.Email.Should().Be("def");
        }
    }

    [TestCase("abc", "def", "abc def")]
    [TestCase("abc", "", "abc")]
    [TestCase("", "def", "def")]
    public void DisplayName_ExpectedBehavior(string givenName, string familyName, string expected)
    {
        var staffViewDto = new StaffViewDto { GivenName = givenName, FamilyName = familyName };

        staffViewDto.Name.Should().Be(expected);
    }

    [TestCase("abc", "def", "abc def")]
    [TestCase("abc", "", "abc")]
    [TestCase("", "def", "def")]
    public void DisplayNameWithOffice_ExpectedBehavior(string givenName, string familyName, string expectedDisplayName)
    {
        var staffViewDto = new StaffViewDto
        {
            Active = true,
            GivenName = givenName,
            FamilyName = familyName,
            Office = new OfficeDisplayViewDto { Id = Guid.Empty, Name = TextData.Phrase },
        };

        staffViewDto.DisplayNameWithOffice.Should().Be($"{expectedDisplayName} ({TextData.Phrase})");
    }

    [TestCase("abc", "def", "abc def")]
    [TestCase("abc", "", "abc")]
    [TestCase("", "def", "def")]
    public void DisplayNameWithOffice_NullOffice_ExpectedBehavior(
        string givenName,
        string familyName,
        string expectedDisplayName)
    {
        var staffViewDto = new StaffViewDto
        {
            Active = true,
            GivenName = givenName,
            FamilyName = familyName,
        };

        staffViewDto.DisplayNameWithOffice.Should().Be(expectedDisplayName);
    }

    [TestCase("abc", "def", "abc def")]
    [TestCase("abc", "", "abc")]
    [TestCase("", "def", "def")]
    public void DisplayNameWithOffice_InactiveUser_ExpectedBehavior(
        string givenName,
        string familyName,
        string expectedDisplayName)
    {
        var staffViewDto = new StaffViewDto
        {
            Active = false,
            GivenName = givenName,
            FamilyName = familyName,
            Office = new OfficeDisplayViewDto { Id = Guid.Empty, Name = TextData.Phrase },
        };

        staffViewDto.DisplayNameWithOffice.Should().Be($"{expectedDisplayName} ({TextData.Phrase}) [Inactive]");
    }

    [TestCase("abc", "def", "def, abc")]
    [TestCase("abc", "", "abc")]
    [TestCase("", "def", "def")]
    public void SortableFullName_ExpectedBehavior(string givenName, string familyName, string expected)
    {
        var staffViewDto = new StaffViewDto { GivenName = givenName, FamilyName = familyName };
        staffViewDto.SortableFullName.Should().Be(expected);
    }

    [Test]
    public void AsUpdateDto_ExpectedBehavior()
    {
        var staffViewDto = new StaffViewDto
        {
            Active = true,
            Phone = TestConstants.ValidPhoneNumber,
            Office = new OfficeDisplayViewDto { Id = Guid.NewGuid() },
        };

        var result = staffViewDto.AsUpdateDto();

        using (new AssertionScope())
        {
            result.Active.Should().BeTrue();
            result.Phone.Should().Be(staffViewDto.Phone);
            result.OfficeId.Should().Be(staffViewDto.Office.Id);
        }
    }
}
