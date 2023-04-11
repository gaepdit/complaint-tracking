using Cts.Domain.Entities.Offices;
using Cts.TestData;

namespace EfRepositoryTests.Offices;

public class GetStaffMembersList
{
    private IOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenStaffExist_ReturnsList()
    {
        // First active office was seeded with staff.
        var item = OfficeData.GetOffices.First(e => e.Active);
        var result = await _repository.GetStaffMembersListAsync(item.Id, false);
        result.Should().BeEquivalentTo(item.StaffMembers,
            opts => opts.Excluding(e => e.Office)
        );
    }

    [Test]
    public async Task WhenStaffDoNotExist_ReturnsEmptyList()
    {
        var item = OfficeData.GetOffices.Last(e => e.Active);
        var result = await _repository.GetStaffMembersListAsync(item.Id, false);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenOfficeDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.GetStaffMembersListAsync(Guid.Empty, false);
        result.Should().BeEmpty();
    }
}
