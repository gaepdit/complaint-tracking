using Cts.Domain.Entities.Concerns;
using Cts.TestData;

namespace EfRepositoryTests.BaseReadRepository;

public class Find
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
        var result = await _repository.FindAsync(item.Id);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
