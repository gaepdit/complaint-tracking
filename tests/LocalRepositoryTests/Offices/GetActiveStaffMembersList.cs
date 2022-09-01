using Cts.Domain.Entities;
using Cts.LocalRepository.Repositories;
using GaEpd.Library.Domain.Repositories;

namespace LocalRepositoryTests.Offices;

public class GetActiveStaffMembersList
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenStaffExist_ReturnsList()
    {
        // First active office was seeded with active staff.
        var office = _repository.Items.First(e => e.Active);
        var result = await _repository.GetActiveStaffMembersListAsync(office.Id);
        result.Should().BeEquivalentTo(office.StaffMembers);
    }

    [Test]
    public async Task WhenStaffDoNotExist_ReturnsEmptyList()
    {
        var office = _repository.Items.Last();
        var result = await _repository.GetActiveStaffMembersListAsync(office.Id);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenOfficeDoesNotExist_Throws()
    {
        var id = Guid.Empty;
        var action = async () => await _repository.GetActiveStaffMembersListAsync(id);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {id}");
    }
}
