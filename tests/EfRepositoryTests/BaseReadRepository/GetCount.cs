using Cts.Domain.Entities.Concerns;
using Cts.TestData;

namespace EfRepositoryTests.BaseReadRepository;

public class GetCount
{
    private IConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsCount()
    {
        var item = ConcernData.GetConcerns.First();
        var result = await _repository.CountAsync(e => e.Id == item.Id);
        result.Should().Be(1);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsZero()
    {
        var result = await _repository.CountAsync(e => e.Id == Guid.Empty);
        result.Should().Be(0);
    }
}
