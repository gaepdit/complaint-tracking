using GaEpd.AppLibrary.Domain.Repositories;
using MyAppRoot.Domain.Offices;
using MyAppRoot.TestData.Offices;

namespace IntegrationTests.Offices;

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
        // First active office was seeded with active staff.
        var item = OfficeData.GetOffices.First(e => e.Active);
        var result = await _repository.GetActiveStaffMembersListAsync(item.Id);
        result.Should().BeEquivalentTo(item.StaffMembers);
    }

    [Test]
    public async Task WhenStaffDoNotExist_ReturnsEmptyList()
    {
        var item = OfficeData.GetOffices.Last(e => e.Active);
        var result = await _repository.GetActiveStaffMembersListAsync(item.Id);
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
