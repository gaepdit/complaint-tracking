using Cts.Domain.Entities.Concerns;
using Cts.LocalRepository.Repositories;
using GaEpd.AppLibrary.Domain.Repositories;

namespace LocalRepositoryTests.BaseReadRepository;

public class Get
{
    private LocalConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = _repository.Items.First();
        var result = await _repository.GetAsync(item.Id);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_Throws()
    {
        var id = Guid.Empty;
        var action = async () => await _repository.GetAsync(id);
        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(Concern).FullName}, id: {id}");
    }
}
