using Cts.Domain.Entities.Concerns;
using Cts.TestData;
using Cts.TestData.Constants;

namespace EfRepositoryTests.Concerns;

public class FindByName
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
        var result = await _repository.FindByNameAsync(item.Name);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindByNameAsync(TestConstants.NonExistentName);
        result.Should().BeNull();
    }
}
