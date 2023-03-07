using Cts.Domain.Concerns;
using Cts.TestData;
using GaEpd.AppLibrary.Domain.Repositories;

namespace EfRepositoryTests.BaseReadRepository;

public class Get
{
    private IConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = ConcernData.GetConcerns.First(e => e.Active);
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
