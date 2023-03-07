using Cts.Domain.Concerns;
using Cts.TestData;

namespace EfRepositoryTests.BaseReadRepository;

public class Exists
{
    private IConcernRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetConcernRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsTrue()
    {
        var item = ConcernData.GetConcerns.First();
        var result = await _repository.ExistsAsync(e => e.Id == item.Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsFalse()
    {
        var result = await _repository.ExistsAsync(e => e.Id == Guid.Empty);
        result.Should().BeFalse();
    }
}
