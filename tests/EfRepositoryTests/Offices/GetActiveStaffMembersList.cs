using Cts.Domain.Entities.Offices;
using Cts.TestData;

namespace EfRepositoryTests.Offices;

public class GetActiveStaffMembersList
{
    private IOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenStaffExist_ReturnsList()
    {
        var item = OfficeData.GetOffices.First(e => e.Active);
        var result = await _repository.GetActiveStaffMembersListAsync(item.Id);
        result.Should().BeEquivalentTo(item.StaffMembers.Where(e => e.Active),
            options => options.Excluding(u => u.Office));
    }

    [Test]
    public async Task WhenStaffDoNotExist_ReturnsEmptyList()
    {
        var item = OfficeData.GetOffices.Last(e => e.Active);
        var result = await _repository.GetActiveStaffMembersListAsync(item.Id);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenOfficeDoesNotExist_ReturnsEmptyList()
    {
        var id = Guid.Empty;
        var result = await _repository.GetActiveStaffMembersListAsync(id);
        result.Should().BeEmpty();
    }
}
