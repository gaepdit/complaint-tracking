using Cts.Domain.Entities;
using Cts.LocalRepository;
using GaEpd.Library.Domain.Repositories;

namespace LocalRepositoryTests.Offices;

public class GetActiveUsersList
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenUsersExist_ReturnsList()
    {
        // First active office was seeded with active users.
        var office = _repository.Items.First(e => e.Active);
        var result = await _repository.GetActiveUsersListAsync(office.Id);
        result.Should().BeEquivalentTo(office.Users);
    }

    [Test]
    public async Task WhenUsersDoNotExist_ReturnsEmptyList()
    {
        var office = _repository.Items.Last();
        var result = await _repository.GetActiveUsersListAsync(office.Id);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenOfficeDoesNotExist_Throws()
    {
        var id = Guid.Empty;
        var action = async () => await _repository.GetActiveUsersListAsync(id);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {id}");
    }
}
