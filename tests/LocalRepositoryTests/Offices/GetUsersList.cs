using Cts.Domain.Entities;
using Cts.LocalRepository;
using GaEpd.Library.Domain.Repositories;

namespace LocalRepositoryTests.Offices;

public class GetUsersList
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenUsersExist_ReturnsList()
    {
        var office = _repository.Items.First();
        var result = await _repository.GetUsersListAsync(office.Id);
        result.Should().BeEquivalentTo(office.Users);
    }

    [Test]
    public async Task WhenUsersDoNotExist_ReturnsEmptyList()
    {
        var office = _repository.Items.Last();
        var result = await _repository.GetUsersListAsync(office.Id);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenOfficeDoesNotExist_Throws()
    {
        var id = Guid.Empty;
        var action = async () => await _repository.GetUsersListAsync(id);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {id}");
    }
}
