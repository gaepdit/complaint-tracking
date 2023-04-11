using Cts.LocalRepository.Identity;
using Cts.LocalRepository.Repositories;
using Cts.TestData.Identity;

namespace LocalRepositoryTests.Offices;

public class GetStaffMembersList
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenStaffExist_ReturnsList()
    {
        // First active office was seeded with staff.
        var item = _repository.Items.First(e => e.Active);
        var result = await _repository.GetStaffMembersListAsync(item.Id, false);
        result.Should().BeEquivalentTo(UserData.GetUsers.Where(e => e.Office?.Id == item.Id));
    }

    [Test]
    public async Task WhenStaffDoNotExist_ReturnsEmptyList()
    {
        var item = _repository.Items.Last();
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
